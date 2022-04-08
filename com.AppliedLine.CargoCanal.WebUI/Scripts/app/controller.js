//var serverUrl = 'http://localhost:52931';
var serverUrl = 'https://appliedline.com/cargocanalapi';
var api = serverUrl + '/api';

(function () {
    'use strict';

    app.controller('mainCtrl', ['$scope', '$rootScope', '$state', '$http', '$sessionStorage', 'localize', 'refresher', 'appFactory', 'passwordFactory', 'signalRHubProxy',
        function ($scope, $rootScope, $state, $http, $sessionStorage, localize, refresher, appFactory, passwordFactory, signalRHubProxy) {
            //fixes angular refresh page (all local variables lose data) issue
            refresher.refreshApp();

            // hide subscription alert modal
            $rootScope.closeSubscritpionAlert = () => {
                $('#modalSubscriptionAlert').modal('hide');
            };

            $rootScope.googleApiKey = 'AIzaSyCwTw8Tyx3_bPBoZ43MXVDhY7TRnPvXl7Y';
            $rootScope.host = api.substring(0, api.lastIndexOf('/'));

            $scope.dateIncrement = function (date, days) {
                let newDate = new Date(date);
                newDate.setDate(newDate.getDate() + days);
                return newDate;
            };

            $rootScope.days = [
                { label: '7 DAYS', value: 7 },
                { label: '30 DAYS', value: 30 },
                { label: '90 DAYS', value: 90 },
                { label: '180 DAYS', value: 180 },
                { label: '365 DAYS', value: 365 }
            ];

            // TODO: Get User Preference Cookie if it exists
            //       e.g. Language

            //// logout of the app, go to home
            //// clear all saved cookies, sessions, rootscope
            //$scope.logout = function () {
            //    $sessionStorage.__user = $rootScope.User = undefined;
            //    $state.go('home');
            //};

            $scope.regexUrl = "https?://[a-zA-Z]+.+\\.[a-zA-Z]{2,}"; // e.g. http://test.com or https://test.com
            $scope.regexEmail = "[a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\\.[a-zA-Z]{2,}";
            $scope.regexNumber = "\\d+";
            $scope.regexPhone = "\\+?\\d+";
            $scope.regexTin = /^\d{10}$/;



            $scope.prepCards = appFactory.prepCards;

            // used to validate the consignee tin on import / export entry
            $rootScope.validateTin = function (tin, state) {
                appFactory.validateTin(tin, state);
            };

            // returns true if TIN is valid
            $rootScope.getIsTinValid = function () {
                return appFactory.getIsTinValid();
            };

            // set TIN as valid
            $rootScope.setIsTinValid = function (bool) {
                appFactory.setIsTinValid(bool);
            };

            $rootScope.closeWindow = function () {
                $rootScope.showWindow = false;
                appFactory.setModalOpen(false);
            };

            $scope.setLanguage = function () {
                localize.setLanguage($scope.lang);
                if (localize.language === 'am-ET') {
                    // call global variable in script.js to enable Amharic writing
                    $sessionStorage.amET = true;
                } else {
                    $sessionStorage.amET = undefined;
                }
            };

            if ($sessionStorage.amET) {
                $scope.lang = 'am-ET';
                $scope.setLanguage();
            }
            else {
                localize.initLocalizedResources();
                $scope.lang = localize.language;
            }


            // SIGNALR CODES
            var clientProxy = signalRHubProxy(serverUrl, 'accountHub');
            clientProxy.on('toggleUserAccess',
                function (data) {
                    if ($rootScope.User && $rootScope.User.Login.ID === data.ID) {
                        $scope.logout();
                    }
                });



            // TODO: Move common functions to factory
            // ----------------- COMMON FUNCTIONS ---------------------------------------

            // returns a class for transport icons
            $scope.getMotIcon = function (mot) {
                switch (mot.toLowerCase()) {
                    case 'air': return 'common-sprite airplane-32';
                    case 'pipe': return 'common-sprite pipe-32';
                    case 'rail': return 'common-sprite railway-32';
                    case 'sea': return 'common-sprite ship-32';
                    case 'truck': return 'common-sprite truck-32';
                }
            };


            $scope.isNotFF = true; // set this flag to true if you don't want to see the add status button
            $rootScope.refresh = function () {
                $state.reload();
            };


            // no internet connection
            $rootScope.noInternet = function () {
                appFactory.showDialog('No internet connection. Please fix your internet connection and try again.');
            };
        }]);

    app.controller('passwordCtrl', ['$scope', '$state', '$rootScope', '$location', 'passwordFactory', 'appFactory', function ($scope, $state, $rootScope, $location, passwordFactory, appFactory) {
        if ($rootScope.User || $rootScope.User !== undefined) $state.go('home'); // user already logged in

        $scope.queryString = $location.search();

        // verify link validity if it exists
        if ($scope.queryString.uid && $scope.queryString.usalt) {
            appFactory.showLoader('validating password reset link...');

            passwordFactory.checklink($scope.queryString)
                .then(function (response) {
                    if (response.status === 200) {
                        // link is valid
                        $scope.passwordReset.data = {
                            Url: $location.absUrl(),
                            Uid: $scope.queryString.uid,
                            Usalt: $scope.queryString.usalt
                        };

                        appFactory.closeLoader();
                        $state.go('passwordreset.changepassword');
                    }
                    else {
                        // TODO: link no longer exists redirect to 404 page
                        appFactory.closeLoader();
                    }
                });
        }

        $scope.passwordReset = {
            data: {},
            reset: function () {
                $scope.passwordReset.data.Url = $location.absUrl();
                $scope.passwordReset.data.Referrer =
                    $scope.passwordReset.data.Url.substring(0,
                        $scope.passwordReset.data.Url.indexOf($location.url()));

                passwordFactory.reset($scope.passwordReset.data)
                    .then(function (response) {
                        // response returned from the server
                        if (response.status === 200) {
                            appFactory.showDialog('A reset link will be sent to the email address <b class="text-primary">'
                                + $scope.passwordReset.data.Username
                                + '</b> if it exists on CargoCanal.');
                            $state.go('home');

                        } else if (response.status === -1) {
                            $rootScope.noInternet();
                        } else {
                            appFactory.showDialog('Oops! Something terribly went wrong. Please try again later.');
                        }
                    });
            },
            changePassword: function () {
                passwordFactory.change($scope.passwordReset.data)
                    .then(function (response) {
                        if (response.status === 200) {
                            appFactory.showDialog('Password changed successfully.');
                            $state.go('login');
                        }
                        else {
                            appFactory.showDialog('Password change failed.', true);
                        }
                    });
            }
        };

    }]);

    app.controller('importController', ['$scope', '$rootScope', '$http', '$sessionStorage', '$state', 'appFactory', 'Upload', '$timeout', '$filter',
        function ($scope, $rootScope, $http, $sessionStorage, $state, appFactory, Upload, $timeout, $filter) {
            // TODO: go home if the user does not need to see this e.g. bank, gov, cc, consignee
            // go to home, if the user is not logged in
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');

            //$scope.retainDesc = function () {
            //    if ($('#Description').length !== 0) {
            //        // get the description value with jquery and assign it manually
            //        var desc = $('#Description').val();
            //        $scope.newImport.Items[0].Description = desc;
            //    }
            //}

            // this determines what status is available for selection
            $scope.impExpType = 1;


            // this method is called when add import is clicked
            // it gets few of the value data required in the forms
            $scope.startImport = function () {
                appFactory.getCarriers();
                appFactory.getCountries();
                appFactory.getImportExportReasons();
                appFactory.getIncoTerms();
                appFactory.getMots();
            };

            // loads all required values and clean up
            $scope.startImport();

            $scope.filterCarrier = function (model) {
                $scope.motFilter = { 'ModeOfTransportID': $scope.newImport.ModeOfTransportID.toString() };

                // remove Carrier and Vessel from model if pipe 4, truck 3
                if (model.ModeOfTransportID === '4' || model.ModeOfTransportID === '3') {
                    $scope.hideCarrier = true;
                    //model.VesselID = undefined; // auto-complete implementaion for vessel
                    model.Vessel = undefined;
                    model.CarrierID = undefined;
                    return;
                }

                $scope.hideCarrier = false;
            };


            // show the window to add item with details info
            // create a blank item object
            $scope.addNewItem = function () {
                $scope.disableAddToItems = false;
                $rootScope.showWindow = true;
                appFactory.setModalOpen(true);
                $scope.newItem = {
                    ItemDetails: []
                };


                // get all cargos, units, stuffmodes and destination
                appFactory.getCargos();
                appFactory.getUnits();
                appFactory.getStuffModes();
                appFactory.getLocations($rootScope.User.Company.CountryID);
            };

            // adds the new item to the items collection
            $scope.addNewItemToItems = function () {
                $scope.validateItemDetails(); // checks item details exists as required
                if ($scope.disableAddToItems) return;
                $scope.newImport.Items.push($scope.newItem);
                $rootScope.closeWindow();
            };

            // adds blank itemDetail object for creating
            $scope.initItemDetail = function () {
                // don't allow new item detail if the quantity limit is reached
                if ($scope.newItem.Quantity > 0 &&
                    $scope.newItem.ItemDetails.length === $scope.newItem.Quantity) {
                    return;
                }

                $scope.newItemDetail = {};
                $scope.showItemDetailEntry = true;
            };

            // adds itemDetail object to item.ItemDetails
            $scope.addItemDetailToItem = function () {
                // check for duplicate
                for (var i in $scope.newItem.ItemDetails) {
                    if ($scope.newItem.ItemDetails[i].ItemNumber.toLowerCase() === $scope.newItemDetail.ItemNumber.toLowerCase()) {
                        $scope.itemDetailsMsg = 'Item detail already contain packing number "' + $scope.newItemDetail.ItemNumber + '".';
                        $scope.disableAddToItems = true;
                        return;
                    }
                }
                $scope.newItem.ItemDetails.push($scope.newItemDetail);
                $scope.showItemDetailEntry = false;
                if ($scope.disableAddToItems) $scope.disableAddToItems = false;
            };

            // validate item.ItemDetails has atleast 1 count
            $scope.validateItemDetails = function () {
                if ($scope.newItem.ItemDetails.length === 0) {
                    $scope.disableAddToItems = true;
                    $scope.itemDetailsMsg = 'Item detail information is required.';
                    return;
                }

                // for container = 1 and vehicle = 4, 
                // user must enter item detail for quantity 20 and below
                if (($scope.newItem.CargoID.toString() === "1" || $scope.newItem.CargoID.toString() === "4")
                    && $scope.newItem.Quantity < 21 && $scope.newItem.Quantity !== $scope.newItem.ItemDetails.length) {
                    $scope.disableAddToItems = true;
                    $scope.itemDetailsMsg = 'You need to enter ' + $scope.newItem.Quantity + ' cargo details.';
                    return;
                }
                else if (($scope.newItem.CargoID.toString() === "1" || $scope.newItem.CargoID.toString() === "4")
                    && $scope.newItem.Quantity > 20 && $scope.newImport.Documents.length === 0) {
                    // verify an attachment exists for container = 1 and vehicle = 4, where quantity > 20
                    $scope.disableAddToItems = true;
                    $scope.itemDetailsMsg = 'You need to enter ' + $scope.newItem.Quantity + ' cargo details, otherwise, attach supporting documents.';
                    return;
                }

                $scope.disableAddToItems = false;
            };


            // attach file to import/export
            $scope.uploadFiles = function (files, errFiles) {
                $scope.existDocCount = $scope.newImport.Documents.length;
                $scope.files = files;
                $scope.errFiles = errFiles;

                angular.forEach(files, function (file) {
                    if ($scope.existDocCount === 0) {

                        file.upload = Upload.upload({
                            url: api + '/importexport/postdocument',
                            data: { file: file }
                        });

                        file.upload.then(function (response) {
                            $timeout(function () {
                                // returns the document ID
                                file.result = response.data;
                                // attach new importExportDoc
                                $scope.newImport.Documents.push({ DocumentID: file.result });

                            });
                        }, function (response) {
                            if (response.status > 0)
                                $scope.errorMsg = response.status + ': ' + response.data;
                        }, function (evt) {
                            file.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
                        });
                    }
                    else $scope.existDocCount -= 1;
                });
            };

            // removes attachment from UI and server
            $scope.removeAttachment = function (index) {
                var file = $scope.files[index];
                // delete file from server using document id -> file.result
                $http({
                    method: 'DELETE',
                    url: api + '/importexport/deletedocument?id=' + file.result
                });

                // remove file from collections
                $scope.files.splice(index, 1);
                $scope.newImport.Documents.splice(index, 1);
            };

            // removes itemDetail object from item.ItemDetails
            $scope.removeItemDetail = function (index) {
                $scope.newItem.ItemDetails.splice(index, 1);
            };

            // hide the itemDetail entry 
            $scope.hideItemDetailEntry = function () {
                $scope.showItemDetailEntry = false;
            };

            // remove an item from the items collection
            $scope.removeItem = function (index) {
                $scope.newImport.Items.splice(index, 1);
            };

            // init variables for add import
            $scope.initImport = function () {
                appFactory.setIsTinValid(true);
                $scope.disableButton = false; // enable the send button

                // Create new import object
                $scope.newImport = {
                    CompanyID: $rootScope.User.Person.CompanyID,
                    CountryID: $rootScope.User.Company.CountryID,
                    CreatedBy: $rootScope.User.Person.ID,
                    Import: {},
                    Items: [],
                    LC: {},
                    Documents: []
                };

                // attachment model
                $scope.files = undefined;
            };

            // send the new Import collection
            $scope.sendImport = function () {
                // disable the send button
                $scope.disableButton = true;

                $http({
                    method: 'POST',
                    url: api + '/importexport/postimport',
                    data: $scope.newImport,
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        // go to the import list page and refresh the list
                        appFactory.showDialog('Import submitted successfully.');
                        $state.go('import');
                    },
                        function (error) {
                            $scope.disableButton = false; // enable the send button
                            appFactory.showDialog('Import was not submitted.', true);
                        });
            };

            // init import then load existing import collection
            $scope.imports = [];
            $scope.getImports = function () {
                // get ongoing imports for company
                appFactory.getImports($scope.imports.length, $scope.imports)
                    .then(function (data) {
                        if (data !== null) {
                            $scope.imports = data.value;
                            $scope.groupedImports = $filter('groupByDate')($scope.imports, 'DateInserted');
                            $scope.odataInfo = data.odataInfo;
                            appFactory.prepCards();
                        }
                    });
            };

            $scope.getImports();

        }]);

    app.controller('exportController', ['$scope', '$rootScope', '$http', '$sessionStorage', '$state', 'appFactory', 'Upload', '$timeout', '$filter',
        function ($scope, $rootScope, $http, $sessionStorage, $state, appFactory, Upload, $timeout, $filter) {
            // go to home, if the user is not logged in
            // TODO: go home if the user does not need to see this e.g. bank, gov, cc, consignee
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');


            // hides the destination select box when adding items
            $scope.exportmode = true;

            // this determines what status is available for selection
            $scope.impExpType = 2;


            // this method is called when add import is clicked
            // it gets few of the value data required in the forms
            $scope.startExport = function () {
                appFactory.getCarriers();
                appFactory.getCountries();
                appFactory.getImportExportReasons();
                appFactory.getIncoTerms();
                appFactory.getMots();
                appFactory.getLocations($rootScope.User.Company.CountryID);
            };

            // loads all required values
            $scope.startExport();

            $scope.filterCarrier = function (model) {
                $scope.motFilter = { 'ModeOfTransportID': $scope.newExport.ModeOfTransportID.toString() };

                // remove Carrier and Vessel from model if pipe 4, truck 3
                if (model.ModeOfTransportID === '4' || model.ModeOfTransportID === '3') {
                    $scope.hideCarrier = true;
                    //model.VesselID = undefined; // auto-complete implementaion for vessel
                    model.Vessel = undefined;
                    model.CarrierID = undefined;
                    return;
                }

                $scope.hideCarrier = false;
            };


            // show the window to add item with details info
            // create a blank item object
            $scope.addNewItem = function () {
                $scope.disableAddToItems = false;
                $rootScope.showWindow = true;
                appFactory.setModalOpen(true);
                $scope.newItem = {
                    ItemDetails: []
                };

                // get all cargos, units, stuffmodes and destination
                appFactory.getCargos();
                appFactory.getUnits();
                appFactory.getStuffModes();
            };

            // adds the new item to the items collection
            $scope.addNewItemToItems = function () {
                $scope.validateItemDetails(); // checks item details exists as required
                if ($scope.disableAddToItems) return;
                $scope.newExport.Items.push($scope.newItem);
                $rootScope.closeWindow();
            };

            // adds blank itemDetail object for creating
            $scope.initItemDetail = function () {
                $scope.newItemDetail = {};
                $scope.showItemDetailEntry = true;
            };

            // adds itemDetail object to item.ItemDetails
            $scope.addItemDetailToItem = function () {
                // check for duplicate
                for (var i in $scope.newItem.ItemDetails) {
                    if ($scope.newItem.ItemDetails[i].ItemNumber.toLowerCase() === $scope.newItemDetail.ItemNumber.toLowerCase()) {
                        $scope.itemDetailsMsg = 'Item detail already contain packing number "' + $scope.newItemDetail.ItemNumber + '".';
                        $scope.disableAddToItems = true;
                        return;
                    }
                }
                $scope.newItem.ItemDetails.push($scope.newItemDetail);
                $scope.showItemDetailEntry = false;
                if ($scope.disableAddToItems) $scope.disableAddToItems = false;
            };

            // validate item.ItemDetails has atleast 1 count
            $scope.validateItemDetails = function () {
                if ($scope.newItem.ItemDetails.length === 0) {
                    $scope.disableAddToItems = true;
                    $scope.itemDetailsMsg = 'Item detail information is required.';
                    return;
                }

                // for container = 1 and vehicle = 4, 
                // user must enter item detail for quantity 20 and below
                if (($scope.newItem.CargoID.toString() === "1" || $scope.newItem.CargoID.toString() === "4")
                    && $scope.newItem.Quantity < 21 && $scope.newItem.Quantity !== $scope.newItem.ItemDetails.length) {
                    $scope.disableAddToItems = true;
                    $scope.itemDetailsMsg = 'You need to enter ' + $scope.newItem.Quantity + ' cargo details.';
                    return;
                }
                else if (($scope.newItem.CargoID.toString() === "1" || $scope.newItem.CargoID.toString() === "4")
                    && $scope.newItem.Quantity > 20 && $scope.newExport.Documents.length === 0) {
                    // verify an attachment exists for container = 1 and vehicle = 4, where quantity > 20
                    $scope.disableAddToItems = true;
                    $scope.itemDetailsMsg = 'You need to enter ' + $scope.newItem.Quantity + ' cargo details, otherwise, attach supporting documents.';
                    return;
                }
                $scope.disableAddToItems = false;
            };


            // attach file to import/export
            $scope.uploadFiles = function (files, errFiles) {
                $scope.existDocCount = $scope.newExport.Documents.length;
                $scope.files = files;
                $scope.errFiles = errFiles;

                angular.forEach(files, function (file) {
                    if ($scope.existDocCount === 0) {

                        file.upload = Upload.upload({
                            url: api + '/importexport/postdocument',
                            data: { file: file }
                        });

                        file.upload.then(function (response) {
                            $timeout(function () {
                                // returns the document ID
                                file.result = response.data;
                                // attach new importExportDoc
                                $scope.newExport.Documents.push({ DocumentID: file.result });

                            });
                        }, function (response) {
                            if (response.status > 0)
                                $scope.errorMsg = response.status + ': ' + response.data;
                        }, function (evt) {
                            file.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
                        });
                    }
                    else $scope.existDocCount -= 1;
                });
            };

            // removes attachment from UI and server
            $scope.removeAttachment = function (index) {
                var file = $scope.files[index];
                // delete file from server using document id -> file.result
                $http({
                    method: 'DELETE',
                    url: api + '/importexport/deletedocument?id=' + file.result
                });

                // remove file from collections
                $scope.files.splice(index, 1);
                $scope.newExport.Documents.splice(index, 1);
            };

            // removes itemDetail object from item.ItemDetails
            $scope.removeItemDetail = function (index) {
                $scope.newItem.ItemDetails.splice(index, 1);
            };

            // hide the itemDetail entry 
            $scope.hideItemDetailEntry = function () {
                $scope.showItemDetailEntry = false;
            };

            // remove an item from the items collection
            $scope.removeItem = function (index) {
                $scope.newExport.Items.splice(index, 1);
            };

            // init variables for add export
            $scope.initExport = function () {
                appFactory.setIsTinValid(true);
                $scope.disableButton = false; // enable the send button

                // Create new export object
                $scope.newExport = {
                    CompanyID: $rootScope.User.Person.CompanyID,
                    CreatedBy: $rootScope.User.Person.ID,
                    CountryID: $rootScope.User.Company.CountryID,
                    Export: {},
                    Items: [],
                    LC: {},
                    Documents: []
                };

                // attachment model
                $scope.files = undefined;
            };

            // send the new Export collection
            $scope.sendExport = function () {
                $scope.disableButton = true; // disable the send button

                $http({
                    method: 'POST',
                    url: api + '/importexport/postexport',
                    data: $scope.newExport,
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        // go to the import list page and refresh the list
                        appFactory.showDialog('Export submitted successfully.');
                        $state.go('export');
                    },
                        function (error) {
                            $scope.disableButton = false; // enable the send button
                            appFactory.showDialog('Export was not submitted.', true);
                        });
            };


            // init export then load existing export collection
            $scope.exports = [];
            $scope.getExports = function () {
                var skip = $scope.exports.length;
                appFactory.getExports($scope.exports.length, $scope.exports)
                    .then(function (data) {
                        if (data !== null) {
                            $scope.exports = data.value;
                            $scope.groupedExports = $filter('groupByDate')($scope.exports, 'DateInserted');
                            $scope.odataInfo = data.odataInfo;
                            appFactory.prepCards();
                        }
                    });
            };

            // get most recent exports for company
            $scope.getExports();

        }]);

    app.controller('importExportCtrl', ['$scope', '$rootScope', '$http', '$state', '$filter', 'appFactory',
        function ($scope, $rootScope, $http, $state, $filter, appFactory) {
            // controller to handle status, problem updates for importExport documents
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');


            $scope.searchImports = function (searchIsNew) {
                if ($scope.searchText === undefined || $scope.searchText === '') return;
                if (searchIsNew) {
                    $scope.imports = [];
                    $scope.groupedImports = [];
                }

                appFactory.getImports($scope.imports.length, $scope.imports, $scope.searchText)
                    .then(function (data) {
                        if (data !== null) {
                            $scope.imports = data.value;
                            $scope.groupedImports = $filter('groupByDate')($scope.imports, 'DateInserted');
                            $scope.odataInfo = data.odataInfo;
                            appFactory.prepCards();
                        }
                    });
            };

            $scope.searchExports = function (searchIsNew) {
                if ($scope.searchText === undefined || $scope.searchText === '') return;
                if (searchIsNew) {
                    $scope.exports = [];
                    $scope.groupedExports = [];
                }

                appFactory.getExports($scope.exports.length, $scope.exports, $scope.searchText)
                    .then(function (data) {
                        if (data !== null) {
                            $scope.exports = data.value;
                            $scope.odataInfo = data.odataInfo;
                            $scope.groupedExports = $filter('groupByDate')($scope.exports, 'DateInserted');
                            appFactory.prepCards();
                        }
                    });
            };

            function confirmDoneOrRecycle(d, pindex, cindex, model, message, fnc) {
                $scope.confirmed = false;
                if (!d && model) {
                    d = model.d;
                    $scope.confirmed = true; // user confirmed
                    $rootScope.closeDialog(); // close the confirm dialog
                }

                // d should exist by now
                if (!d || $scope.invalidOperation(d)) {
                    return undefined;
                }


                // show confirm dialog before recycle
                if (!$scope.confirmed) {
                    appFactory.showDialog(message.replace('#DOC#', d.Bill || d.WayBill), null, true, fnc);
                    model = {
                        d: d,
                        pindex: pindex,
                        cindex: cindex
                    };
                }

                return model;
            }

            // mark an import/export as completed
            $scope.markAsDone = function (d, pindex, cindex) {

                $scope.doneData = confirmDoneOrRecycle(d,
                    pindex,
                    cindex,
                    $scope.doneData,
                    'You will no longer be able to update this document (#DOC#) if you mark it as done. Would you like to proceed?',
                    $scope.markAsDone);

                if (!$scope.confirmed) return false;

                $http({
                    method: 'GET',
                    url: api + '/importexport/importexportmarkasdone?id=' + $scope.doneData.d.ID,
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        if ($scope.exports === undefined)
                            $scope.groupedImports[$scope.doneData.pindex].value[$scope.doneData.cindex].Completed = true;
                        else
                            $scope.groupedExports[$scope.doneData.pindex].value[$scope.doneData.cindex].Completed = true;

                        appFactory.showDialog('Document marked as done.');
                    }, function (error) {
                        switch (error.data.Message) {
                            case 'ERROR_STATUS_NOT_DELIVERED':
                                appFactory.showDialog('Cargo status must be delivered to mark as done.', true);
                                break;
                            case 'ERROR_STATUS_NOT_LOADED':
                                appFactory.showDialog('Cargo status must be loaded to mark as done.', true);
                                break;
                            case 'ERROR_PROBLEM_NOT_RESOLVED':
                                appFactory.showDialog('Unresolved problem exist. Close all problems to mark as done.', true);
                                break;
                        }
                    });
            };

            // terminate an importExport document and remove from collection
            $scope.terminateImportExport = function (d, pindex, cindex) {

                $scope.recycleData = confirmDoneOrRecycle(d,
                    pindex,
                    cindex,
                    $scope.recycleData,
                    'You will no longer be able to update this document (#DOC#) if recycled. Continue recycle on document',
                    $scope.terminateImportExport);

                if (!$scope.confirmed) return false;

                $http({
                    method: 'POST',
                    url: api + '/importexport/importexportterminate',
                    data: { ID: $scope.recycleData.d.ID, Token: $rootScope.User.Login.Token },
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        // remove the document from the collection
                        if ($scope.exports === undefined)
                            $scope.groupedImports[$scope.recycleData.pindex].value[$scope.recycleData.cindex].Terminated = true; // is imports
                        else
                            $scope.groupedExports[$scope.recycleData.pindex].value[$scope.recycleData.cindex].Terminated = true; // is exports

                        appFactory.showDialog('Document has been recycled.');
                    }, function (error) {
                        if (error.status === -1) {
                            // connection broken
                            appFactory.showDialog('Connection Broken.<br><br>Check your internet cable or WiFi.', true);
                        }
                        else {
                            switch (error.data.Message) {
                                case 'ERROR_DATA_NOTFOUND':
                                    appFactory.showDialog('The document does not exist.<br>Make sure you are connected to the internet by refreshing this page.', true);
                                    break;
                                case 'ERROR_USER_INVALID_TOKEN':
                                default:
                                    appFactory.showDialog('Your session has been terminated due to invalid authentication.', true);
                                    $scope.logout();
                                    break;
                            }
                        }
                    });
            };

            $scope.ieStatues = [];

            // Problem Update functions
            $scope.problem = {
                add: function () {
                    $scope.problem.newProblem = {
                        Messages: [],
                        ImportExportID: $scope.problem.importExport.ID
                    };

                    $scope.problem.showFilters = true;
                    appFactory.getProblems();
                },
                closeWindow: function () {
                    // close the status window
                    $scope.problem.openWindow = false;
                    appFactory.setModalOpen(false);
                },
                data: [],
                getProblems: function (o) {
                    // get statues for the import export id i.e. o.ID
                    $scope.problem.openWindow = true;
                    appFactory.setModalOpen(true);
                    $scope.problem.importExport = o;
                    $http({
                        method: 'GET',
                        url: api + '/importexport/getproblemupdates/' + o.ID,
                        headers: { 'Content-Type': 'application/json; charset=utf-8' }
                    })
                        .then(function (response) {
                            $scope.problem.data = response.data;
                        });
                },
                hideFilters: function () {
                    $scope.problem.showFilters = false;
                },
                importExport: {},
                newProblem: {},
                openWindow: false,
                resolve: function (o) {

                    $http({
                        method: 'PUT',
                        url: api + '/importexport/putproblemupdateresolve',
                        data: o,
                        headers: { 'Content-Type': 'application/json; charset=utf-8' }
                    })
                        .then(function (response) {
                            o.IsResolved = true;
                            o.ResolvedDate = response.data.ResolvedDate;
                            $scope.problem.importExport.ProblemsUnresolved--;
                        });
                },
                delete: function (o) {
                    $http({
                        method: 'DELETE',
                        url: api + '/importexport/deleteproblemupdate',
                        data: { ID: o.ID, Token: $rootScope.User.Login.Token },
                        headers: { 'Content-Type': 'application/json; charset=utf-8' }
                    })
                        .then(function (response) {
                            // remove status at index 0
                            $scope.problem.data.splice(0, 1);
                            if ($scope.problem.importExport.ProblemsUnresolved > 0) {
                                $scope.problem.importExport.ProblemsUnresolved--;
                            }
                            appFactory.showDialog('Problem <b>' + o.ProblemName + '</b> deleted successfully');
                        },
                            function (error) {
                                appFactory.showDialog('Unable to delete problem.', true);
                            });
                },
                save: function () {
                    $http({
                        method: 'POST',
                        url: api + '/importexport/postproblemupdate',
                        data: $scope.problem.newProblem,
                        headers: { 'Content-Type': 'application/json; charset=utf-8' }
                    })
                        .then(function (response) {
                            // reload all problems for the current document, close filter, increment Problems unresolved
                            $scope.problem.showFilters = false;
                            $scope.problem.data = response.data;
                            $scope.problem.importExport.ProblemsUnresolved++;
                        });
                },
                showFilters: false
            };


            // Status Update functions
            $scope.status = {
                add: function () {
                    $scope.status.message = undefined;
                    $scope.status.newStatus = {
                        ImportExportID: $scope.status.importExport.ID
                    };
                    $scope.status.showFilters = true;
                    // load locations and statues from Import or Export controller
                    appFactory.getLocations($rootScope.User.Company.CountryID);
                    appFactory.loadStatuses();
                },
                closeWindow: function () {
                    // close the status window
                    $scope.status.openWindow = false;
                    appFactory.setModalOpen(false);
                },
                data: [],
                getStatuses: function (o) {
                    // get statues for the import export id i.e. o.ID
                    $scope.status.openWindow = true;
                    appFactory.setModalOpen(true);
                    $scope.status.importExport = o; // set the importExport data

                    $http({
                        method: 'GET',
                        url: api + '/importexport/getstatusupdates/' + o.ID,
                        headers: { 'Content-Type': 'application/json; charset=utf-8' }
                    })
                        .then(function (response) {
                            $scope.status.data = response.data;
                        });
                },
                hideFilters: function () {
                    $scope.status.showFilters = false;
                },
                importExport: {},
                newStatus: {},
                openWindow: false,
                delete: function (id) {
                    $http({
                        method: 'DELETE',
                        url: api + '/importexport/deletestatusupdate',
                        data: { ID: id, Token: $rootScope.User.Login.Token },
                        headers: { 'Content-Type': 'application/json; charset=utf-8' }
                    })
                        .then(function (response) {
                            // remove status at index 0
                            $scope.status.data.splice(0, 1);
                            appFactory.showDialog('Status deleted successfully');
                        },
                            function (error) {
                                appFactory.showDialog('Unable to delete status.', true);
                            });
                },
                save: function () {
                    // check if status already in status.data
                    $scope.validStatus = true;
                    var $status = $scope.status.newStatus.StatusID;
                    for (var i in $scope.status.data) {
                        // check the status description does not exist
                        if ($scope.status.data[i].StatusText === $status.Description) {
                            $scope.status.message = 'Status "' + $status.Description + '" already exists.';
                            $scope.validStatus = false;
                            break;
                        }
                    }

                    // verify last status date is less or equal to the new status date
                    if ($scope.status.data.length > 0 && $scope.validStatus) {
                        var dt1 = new Date($scope.status.data[0].StatusDate).toDateString();
                        var dt2 = new Date($scope.status.newStatus.StatusDate).toDateString();
                        var dtDif = new Date(dt2) - new Date(dt1);
                        if (dtDif < 0) {
                            $scope.status.message = 'Status date cannot be less than "' + dt1 + '"';
                            $scope.validStatus = false;
                        }
                    }

                    // check the next allowed import status
                    if (($scope.status.data.length > 0 && $scope.validStatus || $scope.status.data[0] !== undefined && $scope.status.data[0].Abbr === 'CD2')
                        && $scope.impExpType === 1) {
                        var lastStat = $scope.status.data[0];

                        switch (lastStat.Abbr) {
                            case 'CLD' /*cargo loaded*/:
                                if ($status.Abbr !== 'VOY') {
                                    $scope.status.message = 'Status "On Voyage" is required.';
                                    $scope.validStatus = false;
                                }
                                break;

                            case 'VOY' /*on voyage*/:
                                if ($status.Abbr !== 'CDC') {
                                    $scope.status.message = 'Status "Cargo Discharged" is required.';
                                    $scope.validStatus = false;
                                }
                                break;

                            case 'CDC' /*cargo discharged*/:
                                if ($status.Abbr !== 'UCC') {
                                    $scope.status.message = 'Status "Under Custom Clearance" is required.';
                                    $scope.validStatus = false;
                                }
                                break;

                            case 'UCC' /*under custom clearance*/:
                                if ($status.Abbr !== 'UPC') {
                                    $scope.status.message = 'Status "Under Port Clearance" is required.';
                                    $scope.validStatus = false;
                                }
                                break;

                            case 'UPC' /*under port clearance*/:
                                if ($status.Abbr !== 'CRL') {
                                    $scope.status.message = 'Status "Cargo Ready for Loading" is required.';
                                    $scope.validStatus = false;
                                }
                                break;

                            case 'CRL' /*cargo ready for loading*/:
                                if ($status.Abbr !== 'CD1') {
                                    $scope.status.message = 'Status "Cargo Dispatched" is required.';
                                    $scope.validStatus = false;
                                }
                                break;

                            case 'CD1' /*cargo dispatched*/:
                                if ($status.Abbr === 'CD2') break;
                                if ($status.Abbr !== 'DRY') {
                                    $scope.status.message = 'Status "At Dry Port" or "Cargo Delivered" is required.';
                                    $scope.validStatus = false;
                                }
                                break;

                            case 'CD2' /*cargo delivered*/:
                                $scope.status.message = 'Cargo is delivered. You should mark the document as completed.';
                                $scope.validStatus = false;
                                break;

                            case 'DRY' /*at dry port*/:
                                if ($status.Abbr !== 'UC1') {
                                    $scope.status.message = 'Status "Under Custom Inspection" is required.';
                                    $scope.validStatus = false;
                                }
                                break;

                            case 'UC1' /*under custom inspection*/:
                                if ($status.Abbr !== 'CD2') {
                                    $scope.status.message = 'Status "Cargo Delivered" is required.';
                                    $scope.validStatus = false;
                                }
                                break;
                        }
                    }

                    // save if status is valid
                    if ($scope.validStatus) {
                        $scope.status.newStatus.StatusID = $status.ID;

                        $http({
                            method: 'POST',
                            url: api + '/importexport/poststatusupdate',
                            data: $scope.status.newStatus,
                            headers: { 'Content-Type': 'application/json; charset=utf-8' }
                        })
                            .then(function (response) {
                                // reload all statuses for the current document and close filter
                                $scope.status.showFilters = false;
                                $scope.status.data = response.data;
                            });
                    }
                },
                showFilters: false
            };


            $scope.preview = {
                'export': {
                    openWindow: false,
                    data: {},
                    show: function (o) {
                        // get attachments if not exist
                        if (o.Documents === undefined || o.Documents === null) {
                            appFactory.getImportExportDocs(o.ID)
                                .then(function (response) {
                                    if (response.status === 200) o.Documents = response.data;
                                });
                        }

                        $scope.preview.export.data = o;
                        $scope.preview.export.openWindow = true;
                        appFactory.setModalOpen(true);
                    },
                    closeWindow: function () {
                        $scope.preview.export.data = {};
                        $scope.preview.export.openWindow = false;
                        appFactory.setModalOpen(false);
                    }
                },
                'import': {
                    openWindow: false,
                    data: {},
                    show: function (o) {
                        // get attachments if not exist
                        if (o.Documents === undefined || o.Documents === null) {
                            appFactory.getImportExportDocs(o.ID)
                                .then(function (response) {
                                    if (response.status === 200) o.Documents = response.data;
                                });
                        }

                        $scope.preview.import.data = o;
                        $scope.preview.import.openWindow = true;
                        appFactory.setModalOpen(true);
                    },
                    closeWindow: function () {
                        $scope.preview.import.data = {};
                        $scope.preview.import.openWindow = false;
                        appFactory.setModalOpen(false);
                    }
                }
            };

            // Hide the Add Status / Add Problem buttons
            if ($rootScope.User.Company.CompanyTypeID !== 5) $scope.isNotFF = true;
            else $scope.isNotFF = false;

            // Displays the consignee info window for import/export and allows update
            $scope.consigneeInfo = {
                data: {},
                dataRef: {},
                message: '',
                openWindow: false,
                closeWindow: function () {
                    $scope.consigneeInfo.openWindow = false;
                    appFactory.setModalOpen(false);
                },
                linkTinToBill: function () {
                    if ($scope.consigneeInfo.data.Consignee.TIN !== '' || $scope.consigneeInfo.data.Consignee.TIN !== null) {
                        appFactory.linkTinToBill($scope.consigneeInfo.data.Consignee)
                            .then(function (response) {
                                if (response === null) {
                                    $scope.consigneeInfo.message = 'TIN does not exist in the system.';
                                    appFactory.showDialog($scope.consigneeInfo.message, true);
                                    return;
                                }

                                // update view with company name
                                $scope.consigneeInfo.data.Consignee.CompanyName = $scope.consigneeInfo.dataRef.Consignee.CompanyName = response.CompanyName;
                                $scope.consigneeInfo.data.Consignee.TIN = $scope.consigneeInfo.dataRef.Consignee.TIN = response.TIN;
                                $scope.consigneeInfo.data.Consignee.ContactName = $scope.consigneeInfo.dataRef.Consignee.ContactName = response.ContactName;
                                $scope.consigneeInfo.data.Consignee.ContactMobile = $scope.consigneeInfo.dataRef.Consignee.ContactMobile = response.ContactMobile;
                                $scope.consigneeInfo.data.Consignee.Email = $scope.consigneeInfo.dataRef.Consignee.Email = response.Email;

                                $scope.consigneeInfo.message = 'TIN updated successfully.';
                                appFactory.showDialog($scope.consigneeInfo.message);
                            });
                    }
                },
                save: function () {
                    if ($scope.consigneeInfo.data.LC === undefined) $scope.consigneeInfo.data.LC = {};
                    $scope.consigneeInfo.data.LC.ImportExportID = $scope.consigneeInfo.data.Consignee.ImportExportID;

                    appFactory.saveLc($scope.consigneeInfo.data.LC)
                        .then(function (response) {
                            $scope.consigneeInfo.message = 'LC updated successfully.';
                            if (response === null) {
                                appFactory.showDialog('Failed to update LC.', true);
                            }
                            else {
                                appFactory.showDialog($scope.consigneeInfo.message);
                                $scope.consigneeInfo.data.LC = $scope.consigneeInfo.dataRef.LC = response.data;
                            }
                            $scope.consigneeInfo.linkTinToBill();
                        });
                },
                show: function (o) {
                    if ($scope.isNotFF) {
                        $scope.invalidOperation();
                        return;
                    }
                    $scope.consigneeInfo.data = angular.copy(o); // creates a deep copy of original model
                    $scope.consigneeInfo.dataRef = o; // maps a relationship to existing data
                    $scope.consigneeInfo.message = '';
                    $scope.consigneeInfo.openWindow = true;
                    appFactory.setModalOpen(true);
                }
            };


            // Displays the cost info window for import/export
            $scope.costInfo = {
                openWindow: false,
                data: {},
                dataRef: {},
                closeWindow: function () {
                    $scope.costInfo.openWindow = false;
                    appFactory.setModalOpen(false);
                },
                save: function () {
                    // set the ImportExportID and save the cost
                    $scope.costInfo.data.Cost.ImportExportID = $scope.costInfo.data.ID;
                    $scope.costInfo.data.Cost.BaseCurrency = $rootScope.exRates.base;

                    appFactory.saveCostInfo($scope.costInfo.data.Cost)
                        .then(function (data) {
                            if (data === null | undefined) {
                                $scope.costInfo.message = 'Oops! failed to update';
                                return;
                            }

                            // update Cost ID if empty
                            if (isNaN($scope.costInfo.data.Cost.ID) || $scope.costInfo.data.Cost.ID === 0) {
                                $scope.costInfo.data.Cost.ID = data;
                            }

                            // upate the existing import/export cost
                            appFactory.showDialog('Cost updated successfully.');

                            if ($scope.exports === undefined) {
                                $scope.imports[$scope.costInfo.dataIndex].Cost = $scope.costInfo.data.Cost;
                            }
                            else {
                                $scope.exports[$scope.costInfo.dataIndex].Cost = $scope.costInfo.data.Cost;
                            }

                        });
                },
                show: function (d, index) {
                    if ($scope.invalidOperation(d)) {
                        return false;
                    }
                    if (d.Cost === undefined || d.Cost === null) d.Cost = {};

                    if ($scope.exports === undefined) {
                        $scope.originalData = $scope.imports;
                    } else {
                        $scope.originalData = $scope.exports;
                    }
                    for (let i in $scope.originalData) {
                        if ($scope.originalData.hasOwnProperty(i)) {
                            if ($scope.originalData[i].ID === d.ID) {
                                $scope.originalIndex = i;
                                break;
                            }
                        }
                    }

                    $scope.costInfo.data = angular.copy(d); // creates a deep copy of original model
                    $scope.costInfo.dataIndex = $scope.originalIndex;
                    $scope.costInfo.dataRef = d; // maps a relationship to existing data
                    $scope.costInfo.message = '';
                    $scope.costInfo.openWindow = true;
                    appFactory.setModalOpen(true);

                }
            };

            $scope.invalidOperation = function (d) {
                if ($scope.isNotFF || d.Completed || d.Terminated) {
                    appFactory.showDialog('Invalid operation!<br />You cannot make changes.', true);
                    return true;
                }
                return false;
            };
            // get the exchange rates of the day
            if ($rootScope.exRates === undefined) {
                appFactory.getExRate().done(function () { });
            }

        }]);

    app.controller('maritimeController', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            // Maritime Specific Functions
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);

    app.controller('createCompanyCtrl', ['$timeout', '$scope', '$rootScope', '$sessionStorage', '$http', '$state', 'appFactory',
        function ($timeout, $scope, $rootScope, $sessionStorage, $http, $state, appFactory) {
            // TODO: set user country by default
            // get user country
            //if (navigator.geolocation) {
            //    navigator.geolocation.getCurrentPosition(function (position) {
            //        $.getJSON('https://api.geonames.org/countryCode',
            //            {
            //                lat: position.coords.latitude,
            //                lng: position.coords.longitude,
            //                username: 'cargocanal',
            //                type: 'JSON'
            //            },
            //            function (result) {
            //                console.log(result);
            //            });
            //    });
            //}

            // TODO: alternative get user country. Try https
            //$.getJSON("http://ip-api.com/json/?callback=?", function (response) {
            //    console.log(response);
            //})


            if ($rootScope.User === undefined) {
                $scope._userToken = '';
            }
            else {
                $scope._userToken = $rootScope.User.Login.Token;
            }

            // new company model
            $scope.newCompany = {
                Company: {},
                Login: {
                    Token: $scope._userToken
                },
                Person: {}
            };

            // flag to disable a button
            $scope.disableButton = false;


            $scope.recaptchaValid = false;

            // Google reCaptcha verification callback function
            const recaptchaVerified = function () {
                $('#captcha-error').text('');

                $timeout(() => {
                    $scope.recaptchaValid = true;
                }, 0);
            };
            // store the function in global property 
            // so it could be referenced even when minified
            window['recaptchaVerified'] = recaptchaVerified;


            // function to save the company
            $scope.saveNewCompany = function () {

                // Google reCaptcha validation
                let $captcha = $('#recaptcha'), response = grecaptcha.getResponse();

                if (response.length === 0) {
                    $('#captcha-error').text("reCAPTCHA is mandatory");
                    return;

                } else {
                    $('#captcha-error').text('');
                }


                // const CompanyTypeID
                $scope.newCompany.Company.CompanyTypeID = 6;
                $scope.disableButton = true; // disable button
                $http({
                    method: 'POST',
                    url: api + '/account/postcompany',
                    headers: { 'Content-Type': 'application/json' },
                    data: $scope.newCompany
                })
                    .then(function (response) {
                        appFactory.showDialog('Company account created but needs to be verified. Account verification instructions have been sent to your email (<b class="text-primary">'
                            + $scope.newCompany.Person.Email + '</b>).');
                        // wait 2s then go to login page.
                        setTimeout(function () {
                            $state.go('login');
                        }, 2000);
                    },
                        function (error) {
                            switch (error.status) {
                                case 409 /* conflict */:
                                    appFactory.showDialog('Company name or TIN already exists.', true);
                                    break;
                                default:
                                    switch (error.data.Message) {
                                        case 'ERROR_EMAIL_CONFLICT':
                                            appFactory.showDialog('Company not registered.<br><br><b>Email already exists.</b>',
                                                true);
                                            break;
                                        default:
                                            appFactory.showDialog('Oops! Something went wrong.', true);
                                            break;
                                    }
                                    break;
                            }

                            $scope.disableButton = false; // enable button
                        });
            };

            // get list of countries, company types
            appFactory.getCountries();
            appFactory.getCompanyTypes();


        }]);

    app.controller('createConsigneeCtrl', ['$scope', '$rootScope', '$sessionStorage', '$http', '$state',
        function ($scope, $rootScope, $sessionStorage, $http, $state) {
            $scope.disableCompanyType = true;
            $scope.consigneeCompany = { ID: 6 };

        }]);

    app.controller('accountCtrl', ['$scope', '$rootScope', '$sessionStorage', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $sessionStorage, $http, $state, appFactory) {
            // go to home, if the user is not logged in
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');

            $scope.getIsActiveIcon = function (bool) {
                return appFactory.getActiveIcon(bool);
            };

            $scope.doesServiceExist = function (serviceName) {
                if ($scope.account.subscription.data === undefined || $scope.account.subscription.data.length === 0) return false;
                for (var i in $scope.account.subscription.data) {
                    if ($scope.account.subscription.data[i].SubscriptionName.match(serviceName)) {
                        return true;
                    }
                }

                return false;
            };

            // 
            // <parameter>
            //      photoType: number
            // </parameter>
            // The photoType parameter accepts the following numbers and represent the following
            // 0 = USER_PROFILE_IMAGE
            // 1 = COMPANY_LOGO
            // 
            $scope.setPhotoType = function (photoType) {
                $rootScope.photoType = photoType;
            };

            $scope.account = {
                password: {
                    change: function () {
                        $scope.account.password.openWindow = true;
                        appFactory.setModalOpen(true);
                    },
                    closeWindow: function () {
                        $scope.account.password.openWindow = false;
                        appFactory.setModalOpen(false);
                    },
                    data: {
                        ID: $rootScope.User.Login.ID,
                        Token: $rootScope.User.Login.Token
                    },
                    openWindow: false,
                    save: function () {
                        $http({
                            method: 'PUT',
                            url: api + '/account/changepassword',
                            data: $scope.account.password.data,
                            headers: { 'Content-Type': 'application/json; charset=utf-8' }
                        })
                            .then(function (response) {
                                // password was changed successfully
                                $scope.account.password.closeWindow();
                                $rootScope.User.Login.LastPasswordChange = new Date();
                                $sessionStorage.__user = $rootScope.User;
                                appFactory.showDialog('Password changed successfully.');
                            }, function (error) {
                                appFactory.showDialog('Failed to change password.', true);
                            });
                    }
                },
                profile: {
                    data: {},
                    edit: function () {
                        // enable the input controls and pass data to be bound to the UI
                        $scope.account.profile.editEnabled = true;
                        $scope.account.profile.data = angular.copy($rootScope.User.Person);
                    },
                    editEnabled: false,
                    save: function () {
                        $scope.account.profile.editEnabled = false;

                        $http({
                            method: 'PUT',
                            url: api + '/account/putperson',
                            data: $scope.account.profile.data,
                            headers: { 'Content-Type': 'application/json; charset=utf-8' }
                        })
                            .then(function (response) {
                                // update session storage and rootscope with the data
                                $sessionStorage.__user.Person = $rootScope.User.Person = response.data;
                            });
                    }
                },
                subscription: {
                    // collection of subscriptions
                    data: [],
                    // get the company subscriptions
                    get: function () {
                        appFactory.getSubscriptionTypes();
                        appFactory.getCompanySubscriptions($rootScope.User.Company.ID)
                            .then(function (data) {
                                $scope.account.subscription.data = data;
                            });
                    },
                    set: function (service) {
                        if ($scope.serviceInCategory) {
                            service = $scope.serviceInCategory.service;
                        }

                        // check if company has existing subscription in the same category
                        if (!($scope.account.subscription.data === undefined || $scope.account.subscription.data.length === 0)
                            && ($scope.serviceInCategory === undefined || !$rootScope.confirmMode)) {
                            for (var i in $scope.account.subscription.data) {
                                if ($scope.account.subscription.data[i].Category.match(service.Category) && $scope.account.subscription.data[i].IsActive) {
                                    appFactory.showDialog('Changing subscription to <b>' + service.SubscriptionName
                                        + '</b> would deactivate <b>' + $scope.account.subscription.data[i].SubscriptionName
                                        + '</b>.<br/>Do you agree to this change?',
                                        null, true, $scope.account.subscription.set);

                                    $scope.serviceInCategory = { 'service': service };
                                    break;
                                }
                            }

                        }
                        else {
                            // close dialog if open
                            $rootScope.closeDialog();

                            appFactory.setCompanySubscriptions({
                                'CompanyID': $rootScope.User.Company.ID,
                                'SubcriptionTypeID': service.ID
                            })
                                .then(function (response) {
                                    // clear the serviceInCategory data
                                    $scope.serviceInCategory = undefined;

                                    if (response.status === 200) {
                                        $scope.account.subscription.get();
                                        appFactory.showDialog('Subscription activated successfully.');
                                    }
                                    else {
                                        if (response.data.Message) {
                                            appFactory.showDialog('Subscription already exists.', true);
                                            return;
                                        }
                                        appFactory.showDialog('Unable to activate Subscription.', true);
                                    }
                                });
                        }
                    }
                },
                company: {
                    data: {},
                    edit: function () {
                        // enable the input controls and pass data to be bound to the UI
                        $scope.account.company.editEnabled = true;
                        $scope.account.company.data = angular.copy($rootScope.User.Company);
                    },
                    editEnabled: false,
                    save: function () {
                        $scope.account.company.editEnabled = false;

                        $http({
                            method: 'PUT',
                            url: api + '/account/putcompany',
                            data: $scope.account.company.data,
                            headers: { 'Content-Type': 'application/json; charset=utf-8' }
                        })
                            .then(function (response) {
                                // update session storage and rootscope with the data
                                $sessionStorage.__user.Company = $rootScope.User.Company = response.data;
                            });
                    }
                },
                users: {
                    changeIsActiveState: function (i) {
                        // this makes the user active or inactive
                        $http({
                            method: 'PUT',
                            url: api + '/account/lockorunlockuser',
                            data: $scope.account.users.data[i],
                            headers: { 'Content-Type': 'application/json; charset=utf-8' }
                        })
                            .then(function (response) {
                                $scope.account.users.data[i] = response.data;
                            });
                    },
                    add: function () {
                        $scope.account.users.newUserEnabled = true;
                        $scope.account.roles.getRoles();
                        $scope.Person = { CompanyID: $rootScope.User.Company.ID };
                        $scope.Login = { IsActive: true };
                        $scope.message = undefined;
                    },
                    closeWindow: function () {
                        $scope.account.users.newUserEnabled = false;
                    },
                    data: {},
                    edit: function () {

                    },
                    editEnabled: false,
                    getUsers: function () {
                        $http({
                            method: 'POST',
                            url: api + '/account/getusersincompany',
                            data: $rootScope.User.Login,
                            headers: { 'Content-Type': 'application/json; charset=utf-8' }
                        })
                            .then(function (response) {
                                $scope.account.users.data = response.data;
                            });
                    },
                    newUserEnabled: false,
                    // save the new user for your company
                    save: function () {
                        $scope.account.users.newUserEnabled = false;
                        // attach the creator's token to the Login
                        $scope.Login.Token = $rootScope.User.Login.Token;
                        $scope.newUser = {
                            Login: $scope.Login,
                            Person: $scope.Person
                        };

                        $http({
                            method: 'POST',
                            url: api + '/account/postpersonforcompany',
                            data: $scope.newUser,
                            headers: { 'Content-Type': 'application/json; charset=utf-8' }
                        })
                            .then(function (response) {
                                // user saved - refresh the list of users
                                $scope.account.users.getUsers();
                            }, function (error) {
                                switch (error.status) {
                                    case 400: /* bad request - subscription limit reached */
                                        appFactory.showDialog('Oops! You are unable to create a new user because your have used up your subscription limit. Upgrade to a higher package to enjoy additional benefits.', true);
                                        break;
                                    case 404: /* not found -> no subscription */
                                        appFactory.showDialog('Oops! You are unable to create a new user because your have no active subscription.', true);
                                        break;
                                    case 500: /* internal server error */
                                        // TODO: redirect to a friendly error page
                                        break;
                                    default:
                                }
                            });
                    }
                },
                roles: {
                    add: function () {
                        $scope.account.roles.newRoleEnabled = true;
                        $scope.account.roles.getRoles();
                        $scope.role = { CompanyID: $rootScope.User.Company.ID };
                        $scope.rolePermission = {};
                        $scope.message = undefined;
                    },
                    closeWindow: function () {
                        $scope.account.roles.newRoleEnabled = false;
                    },
                    data: {},
                    getRoles: function () {
                        $http({
                            method: 'POST',
                            url: api + '/account/getrolesincompany',
                            data: $rootScope.User.Login,
                            headers: { 'Content-Type': 'application/json; charset=utf-8' }
                        })
                            .then(function (response) {
                                $scope.account.roles.data = response.data;
                            });
                    },
                    newRoleEnabled: false,
                    save: function () {
                        $scope.account.roles.newRoleEnabled = false;
                        $scope.newRole = {
                            Role: $scope.role,
                            RolePermission: $scope.rolePermission,
                            Token: $rootScope.User.Login.Token
                        };


                        $http({
                            method: 'POST',
                            url: api + '/account/postroleandpermission',
                            data: $scope.newRole,
                            headers: { 'Content-Type': 'application/json; charset=utf-8' }
                        })
                            .then(function (response) {
                                // role saved successfully
                            }, function (error) {
                                switch (error.status) {
                                    case 400: /* bad request -> subscription limit reached */
                                        appFactory.showDialog('Oops! You are unable to create a new role because your have used up your subscription limit. Upgrade to a higher package to enjoy additional benefits.', true);
                                        break;
                                    case 404: /* not found -> no subscription */
                                        appFactory.showDialog('Oops! You are unable to create a new role because your have no active subscription.', true);
                                        break;
                                    case 500: /* internal server error */
                                        // TODO: redirect to a friendly error page
                                        break;
                                    default:
                                }
                            });
                    }
                }
            };

            // get the company subscriptions with marketplace services
            $scope.account.subscription.get();

            // prompt for a mandatory password reset
            if ($rootScope.User.Login.LastSeen === null || $rootScope.User.Login.LastPasswordChange === null) {
                $scope.message = 'To secure your account, kindly change your password.';
                $scope.account.password.change();
            }

        }]);

    app.controller('adminCtrl', ['$scope', '$rootScope', '$http', '$state',
        function ($scope, $rootScope, $http, $state) {
            // pass user token to Web API
            // go to home, if the user is not logged in or does not have edit user privilege
            // just incase the user paste in the url for manage users
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');

            $http({
                method: 'POST',
                url: api + '/account/getuserperm',
                data: $rootScope.User.Login,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    $scope.perms = response.data;
                    if ($scope.perms.EditUser === false)
                        $state.go('home');
                });
        }]);

    app.controller('accountAdminCtrl', ['$scope', '$rootScope', '$sessionStorage', '$http', '$state',
        function ($scope, $rootScope, $sessionStorage, $http, $state) {

            // get users in your company
            $scope.account.users.getUsers();

        }]);

    app.controller('nationalBankCtrl', ['$scope', '$rootScope', '$sessionStorage', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $sessionStorage, $http, $state, appFactory) {
            // ensure only national bank user gets here
            if ($rootScope.User.Company.CompanyTypeID !== 101) $state.go('home');

            $scope.tinOrLcData = [];

            $scope.getImportExportByTinOrLc = function (searchButton) {
                if ($scope.searchText === undefined || $scope.searchText === '') return;

                appFactory.showLoader();
                appFactory.setModalOpen(true);

                appFactory.getImportExportByTinOrLc($scope.tinOrLcData, $scope.searchText, searchButton)
                    .then(function (data) {
                        if (data !== null) {
                            $scope.tinOrLcData = data.value;
                            $scope.odataInfo = data.odataInfo;
                            appFactory.prepCards();
                        }
                        appFactory.setModalOpen(false);
                        appFactory.closeLoader();
                    });
            };
        }]);

    app.controller('consigneeCtrl', ['$scope', '$rootScope', '$sessionStorage', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $sessionStorage, $http, $state, appFactory) {
            // TODO: Take care of users that should have access to this controller
            if ($rootScope.User.Company.CompanyTypeID !== 6) $state.go('home');

            $scope.tinOrLcData = [];

            // get import documents using the customer's TIN
            $scope.getImportExportByTinOrLc = function (searchButton) {
                if (searchButton && ($scope.searchText === undefined || $scope.searchText === '')) return;

                appFactory.showLoader();
                appFactory.setModalOpen(true);

                appFactory.getImportExportByTinOrLc($scope.tinOrLcData, $scope.searchText, searchButton)
                    .then(function (data) {
                        if (data !== null) {
                            $scope.tinOrLcData = data.value;
                            $scope.odataInfo = data.odataInfo;
                            appFactory.prepCards();
                        }

                        appFactory.setModalOpen(false);
                        appFactory.closeLoader();
                    });
            };

            $scope.getImportExportByTinOrLc();
        }]);

    app.controller('reportController', ['$scope', '$rootScope', '$sessionStorage', '$http', '$state', '$filter', 'groupByFactory', 'appFactory',
        function ($scope, $rootScope, $sessionStorage, $http, $state, $filter, groupByFactory, appFactory) {
            // TODO: Take care of users that should have access to this controller
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');


            $scope.report = {
                data: [], // holds report data
                filter: { name: {}, Token: $rootScope.User.Login.Token },
                groupBy: '',
                generate: function () {
                    appFactory.showLoader('generating report...');
                    appFactory.setModalOpen(true);
                    $scope.report.data = [];
                    $scope.report.groupBy = '';
                    $state.go('report'); // ensures dataTable scripts reloaded
                    // make a call WebAPI to generate the report
                    // inject the data in $scope.report.data
                    // go to the state that matches the pattern 'report.{$scope.report.filter.name.value}'
                    $scope.report.filter.ReportName = $scope.report.filter.name.value;

                    $http({
                        method: 'POST',
                        url: api + '/reports/generatereport',
                        data: $scope.report.filter,
                        headers: { 'Content-Type': 'application/json; charset=utf-8' }
                    })
                        .then(function (response) {
                            let actualData = response.data.Table || response.data;
                            $scope.report.data = actualData;
                            //$scope.report.data = $filter('groupByField')(actualData, $scope.report.groupBy);

                            $state.go('report.' + $scope.report.filter.name.value);
                            appFactory.closeLoader();
                            appFactory.setModalOpen(false);
                        }, function (error) {
                            appFactory.closeLoader();
                            appFactory.setModalOpen(false);
                        });
                },
                getTotal: function (o, field) {
                    $scope.total = 0;
                    for (var i in o) {
                        if (o[i][field] !== undefined) $scope.total += o[i][field];
                        else break;
                    }
                    return $scope.total;
                },
                list: [
                    { code: '012', value: 'bill_statuses_grouped_by_tin_report', label: 'Bill Statuses By Forwarder' },
                    { code: '001', value: 'cargo_dispatched_weight_grouped_by_month_report', label: 'Cargo Dispatched Weight By Months' },
                    { code: '002', value: 'cargo_import_weight_grouped_by_tin_report', label: 'Cargo Import Weight By Forwarder' },
                    { code: '003', value: 'cargo_on_voyage_only_grouped_by_country_report', label: 'Cargo On Voyage Only By Country' },
                    { code: '004', value: 'problem_grouped_by_tin_report', label: 'Problems By Forwarder' },
                    { code: '005', value: 'problem_grouped_by_tin_unresolved_report', label: 'Problems By Forwarder Unresolved' },
                    { code: '006', value: 'demurrage_grouped_by_tin_report', label: 'Demurrage By Forwarder' },
                    { code: '007', value: 'demurrage_grouped_by_tin_active_report', label: 'Demurrage By Forwarder Active' },
                    { code: '008', value: 'transit_time_grouped_by_import_report', label: 'Transit Time From Origin By Bill Of Lading' },
                    { code: '008-1', value: 'transit_time_after_discharge_grouped_by_import_report', label: 'Transit Time After Discharge By Bill' },
                    { code: '008-2', value: 'transit_time_after_discharge_grouped_by_discharge_port_report', label: 'Transit Time After Discharge By Port' },
                    { code: '009', value: 'transit_time_grouped_by_country_report', label: 'Transit Time From Origin By Country Detailed' },
                    { code: '010', value: 'transit_time_grouped_by_country_summary_report', label: 'Transit Time From Origin By Country Summary' },
                    { code: '011', value: 'transit_time_grouped_by_tin_report', label: 'Transit Time From Origin By Forwarder' }

                ],
                loadView: function () {
                    // clear data and return to report state
                    $scope.report.data = [];
                    $state.go('report');
                },
                isActiveClass: function (bool) {
                    if (bool) return 'status-icon bg-green';
                    return 'status-icon bg-red';
                }
            };

        }]);


})();