(function () {
    "use strict";

    app.service('refresher', ['$sessionStorage', "$rootScope", function ($sessionStorage, $rootScope) {
        this.refreshApp = function () {

            if ($sessionStorage.__user === undefined) {
                //
            } else {
                $rootScope.User = $sessionStorage.__user;
            }
        };

        this.refreshApp();
    }]);

    app.factory("autoCompleteDataService", ["$http", function ($http) {
        return {
            getSource: function (srcLink, extraparams) {
                // make a call to the external source link eg. /values/querycustomer               
                //return api + '/values/' + srcLink;
                return function (request, response) {
                    $.ajax({
                        url: api + '/' + srcLink, //autoCompleteDataService.getSource(attrs.source),
                        dataType: "json",
                        data: {
                            term: request.term,
                            extraparams: extraparams
                        },
                        success: function (data) {
                            response(data);
                        }
                    });
                };
            }
        };
    }]);

    app.factory('passwordFactory', ['$rootScope', '$http', function ($rootScope, $http) {
        var service = {};

        service.change = function (data) {
            return $http({
                method: 'PUT',
                url: api + '/account/passwordresetchange',
                data: data,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
        };

        service.checklink = function (data) {
            return $http({
                method: 'POST',
                url: api + '/account/passwordresetcheck',
                data: data,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
        };

        service.reset = function (data) {
            return $http({
                method: 'POST',
                url: api + '/account/passwordreset',
                data: data,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
        };

        service.validate = function (password) {
            let minMaxLength = /[\S]{6,32}/,
                upper = /[A-Z]/,
                lower = /[a-z]/,
                number = /[0-9]/,
                special = /[ !"#$%&'()*+,\-./:;<=>?@^]/;

            if (minMaxLength.test(password) &&
                upper.test(password) &&
                lower.test(password) &&
                number.test(password) &&
                special.test(password)
            ) {
                return true;
            }
            return false;
        };

        $rootScope.validatePassword = function (password, model) {
            let className = model.$$element[0].className.includes('textbox-editor') ? ' textbox-editor' : '';
            if (service.validate(password)) {
                model.$error = {};
                model.$invalid = false;
                model.$valid = true;
                model.$$element[0].className = 'form-control ng-not-empty ng-dirty ng-valid-parse ng-valid ng-valid-required ng-touched' + className;
            } else {
                model.$error = { "pattern": true };
                model.$invalid = true;
                model.$valid = false;
                model.$$element[0].className = 'form-control ng-not-empty ng-dirty ng-valid-parse ng-valid ng-valid-required ng-touched ng-invalid' + className;
            }
        };

        return service;
    }]);

    app.factory('appFactory', ['$rootScope', '$state', '$http', '$q', '$sce', function ($rootScope, $state, $http, $q, $sce) {
        $rootScope.modalOpen = false;
        var _isTinValid = true;
        var service = {};
        var odataUrl = api.substring(0, api.indexOf('/api')) + '/odata';

        service.setModalOpen = function (bool) {
            $rootScope.modalOpen = bool;
        };

        service.getPhotoUrl = (url) => {
            if (!url) {
                return '';
            }

            return serverUrl + '/' + url;
        };


        // Service Characters Remaining
        service.xtersLeft = function (maxLength, xterLength) {
            return maxLength - xterLength;
        };
        
        // Service RGB
        service.getRgbArray = function (length) {
            const rgb = [];
            const rgbaOpaque = [];
            const max = 255;
            const min = 1;
            for (let i = 0; i < length; i++) {
                const r = Math.floor(Math.random() * (max - min)) + min;
                const g = Math.floor(Math.random() * (max - min)) + min;
                const b = Math.floor(Math.random() * (max - min)) + min;

                rgb.push('rgb(' + r + ',' + g + ',' + b + ')'); // add new rgb(x,x,x) color
                rgbaOpaque.push('rgba(' + r + ',' + g + ',' + b + ', 0.4)');
            }

            return {
                'rgb': rgb,
                'rgbaOpaque': rgbaOpaque
            };
        }

        // initialize common functions used by multiple controllers
        let _initHelpers = function () {
            $rootScope.getPhotoUrl = service.getPhotoUrl;

            // return a class that sets the <body> overflow: hidden
            $rootScope.modalOpenClass = function () {
                if ($rootScope.modalOpen) return 'fullscreen-open';
                return '';
            };

            // close dialog window
            $rootScope.closeDialog = function () {
                service.setModalOpen(false);
                $rootScope.showDialog = false;
                $rootScope.confirmMode = false;
            };

            // return text in html encoding
            $rootScope.trustEntryAsHtml = function (text) {
                return $sce.trustAsHtml(text);
            };

            // get ports of loading
            $rootScope.getPols = function (countryid, model) {
                model.PortOfLoadingID = undefined;
                $http({
                    method: 'GET',
                    url: api + '/values/getports/' + countryid,
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $rootScope.pols = response.data;
                    });
            };

            // get ports of discharge
            $rootScope.getPods = function (countryid, model) {
                model.PortOfDischargeID = undefined;
                $http({
                    method: 'GET',
                    url: api + '/values/getports/' + countryid,
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $rootScope.pods = response.data;
                    });
            };

            $rootScope.getSubCargos = function (cargoId, model) {
                model.SubCargoID = undefined;
                $http({
                    method: 'GET',
                    url: api + '/values/getsubcargos/' + cargoId,
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $rootScope.cargoSubCargos = response.data;
                    });
            };


            // optionsMore object helpers
            $rootScope.optionsMore = {
                openWindow: false,
                show: function (record, recordIndex, recordChildIndex) {
                    $rootScope.record = record;
                    $rootScope.recordIndex = recordIndex;
                    $rootScope.recordChildIndex = recordChildIndex;
                    $rootScope.optionsMore.openWindow = true;
                    service.setModalOpen(true);
                },
                closeWindow: function () {
                    $rootScope.optionsMore.openWindow = false;
                    service.setModalOpen(false);
                    $rootScope.record = undefined;
                    $rootScope.recordIndex = undefined;
                    $rootScope.recordChildIndex = undefined;
                }
            };


            // comment helper window
            $rootScope.commentInit = function (importExportID) {
                $rootScope.showComment = true;
                service.setModalOpen(true);
                $rootScope.$broadcast('commentOpen', importExportID);
            };



            // status functions helper
            $rootScope.status = {
                openWindow: false,
                showFilters: false,
                closeWindow: function () {
                    // close the status window
                    $rootScope.status.openWindow = false;
                    service.setModalOpen(false);
                },
                data: [],
                getStatuses: function (o) {
                    $rootScope.status.data = [];
                    $rootScope.status.openWindow = true;
                    service.setModalOpen(true);
                    $rootScope.status.importExport = o; // set the importExport data which has the bill number etc
                    $rootScope.status.data = o.StatusesViews;
                }
            };


            // problem functions helper
            $rootScope.problem = {
                openWindow: false,
                showFilters: false,
                //add: function () {
                //    $scope.problem.newProblem = {
                //        Messages: [],
                //        ImportExportID: $scope.problem.importExport.ID,
                //        ProblemDate: new Date(),
                //    };

                //    $scope.problem.showFilters = true;
                //    appFactory.getProblems();
                //},
                closeWindow: function () {
                    // close the status window
                    $rootScope.problem.openWindow = false;
                    service.setModalOpen(false);
                },
                data: [],
                getProblems: function (o) {
                    // get statues for the import export id i.e. o.ID
                    $rootScope.problem.data = [];
                    $rootScope.problem.openWindow = true;
                    service.setModalOpen(true);
                    $rootScope.problem.importExport = o;
                    $http({
                        method: 'GET',
                        url: api + '/importexport/getproblemupdates/' + o.ID,
                        headers: { 'Content-Type': 'application/json; charset=utf-8' }
                    })
                        .then(function (response) {
                            $rootScope.problem.data = response.data;
                        });
                },
                //hideFilters: function () {
                //    $rootScope.problem.showFilters = false;
                //},
                //importExport: {},
                //isProblemsUnresolved: function (o) {
                //    if (o.ProblemsUnresolved == 0) return 'badge icon-data bg-color-grey';
                //    return 'badge icon-data bg-color-red';
                //},
                //newProblem: {},
                //resolve: function (o) {

                //    $http({
                //        method: 'PUT',
                //        url: api + '/importexport/putproblemupdateresolve',
                //        data: o,
                //        headers: { 'Content-Type': 'application/json; charset=utf-8' }
                //    })
                //        .then(function (response) {
                //            o.IsResolved = true;
                //            o.ResolvedDate = response.data.ResolvedDate;
                //            $scope.problem.importExport.ProblemsUnresolved--;
                //        });
                //},
                //delete: function (o) {
                //    $http({
                //        method: 'DELETE',
                //        url: api + '/importexport/deleteproblemupdate',
                //        data: { ID: o.ID, Token: $rootScope.User.Login.Token },
                //        headers: { 'Content-Type': 'application/json; charset=utf-8' }
                //    })
                //        .then(function (response) {
                //            // remove status at index 0
                //            $scope.problem.data.splice(0, 1);
                //            if ($scope.problem.importExport.ProblemsUnresolved > 0) {
                //                $scope.problem.importExport.ProblemsUnresolved--;
                //            }
                //            appFactory.showDialog('Problem <b>' + o.ProblemName + '</b> deleted successfully');
                //        },
                //        function (error) {
                //            appFactory.showDialog('Unable to delete problem.', true);
                //        });
                //},
                //save: function () {
                //    $http({
                //        method: 'POST',
                //        url: api + '/importexport/postproblemupdate',
                //        data: $scope.problem.newProblem,
                //        headers: { 'Content-Type': 'application/json; charset=utf-8' }
                //    })
                //        .then(function (response) {
                //            // reload all problems for the current document, close filter, increment Problems unresolved
                //            $scope.problem.showFilters = false;
                //            $scope.problem.data = response.data;
                //            $scope.problem.importExport.ProblemsUnresolved++;
                //        })
                //},
            };



            // preview function helpers
            $rootScope.preview = {
                'export': {
                    openWindow: false,
                    data: {},
                    show: function (o) {
                        // get attachments if not exist
                        if (o.Documents === undefined || o.Documents === null) {
                            service.getImportExportDocs(o.ID)
                                .then(function (response) {
                                    if (response.status === 200) o.Documents = response.data;
                                });
                        }

                        $rootScope.preview.export.data = o;
                        $rootScope.preview.export.openWindow = true;
                        service.setModalOpen(true);
                    },
                    closeWindow: function () {
                        $rootScope.preview.export.data = {};
                        $rootScope.preview.export.openWindow = false;
                        service.setModalOpen(false);
                    }
                },
                'import': {
                    openWindow: false,
                    data: {},
                    show: function (o) {
                        // get attachments if not exist
                        if (o.Documents === undefined || o.Documents === null) {
                            service.getImportExportDocs(o.ID)
                                .then(function (response) {
                                    if (response.status === 200) o.Documents = response.data;
                                });
                        }

                        $rootScope.preview.import.data = o;
                        $rootScope.preview.import.openWindow = true;
                        service.setModalOpen(true);
                    },
                    closeWindow: function () {
                        $rootScope.preview.import.data = {};
                        $rootScope.preview.import.openWindow = false;
                        service.setModalOpen(false);
                    }
                }
            };

        };

        // dialog windows function
        service.showDialog = function (message, isError, confirmMode, fnPointer) {
            message = isError === true ? '<p class="text-danger">' + message + '</p>' : '<p>' + message + '</p>';

            // add message to existing message if dialog was open
            if ($rootScope.showDialog === true) {
                $rootScope.dialogMsg += message;
                return;
            }

            $rootScope.dialogMsg = message;
            $rootScope.showDialog = true;
            service.setModalOpen(true);

            $rootScope.confirmMode = confirmMode;
            if (confirmMode) $rootScope.confirmDialog = fnPointer;
        };


        // set base64 image to user profile
        service.setDataImage = function () {
            switch ($rootScope.User.Person.Photo) {
                case null: case undefined: case '': break;
                default:
                    $rootScope.User.Person.PhotoDataUrl = 'data:image/jpg;base64,' + $rootScope.User.Person.Photo;
            }

            switch ($rootScope.User.Company.Photo) {
                case null: case undefined: case '': break;
                default:
                    $rootScope.User.Company.PhotoDataUrl = 'data:image/jpg;base64,' + $rootScope.User.Company.Photo;
            }
        };


        // return an icon based on if active is true or false
        service.getActiveIcon = function (active) {
            if (active) return 'status-icon-green';
            return 'status-icon-red';
        };

        // get cargos
        service.getCargos = function () {
            if ($rootScope.cargos === undefined || $rootScope.cargos.length === 0) {
                $http({
                    method: 'GET',
                    url: api + '/values/getcargo?id',
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $rootScope.cargos = response.data;
                    });
            }
        };

        // get units
        service.getUnits = function () {
            if ($rootScope.units === undefined || $rootScope.units.length === 0) {
                $http({
                    method: 'GET',
                    url: api + '/values/getunit?id',
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $rootScope.units = response.data;
                    });
            }
        };

        // get countries
        service.getCountries = function () {
            if ($rootScope.countries === undefined || $rootScope.countries.length === 0) {
                $http({
                    method: 'GET',
                    url: api + '/values/getcountry?id',
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $rootScope.countries = response.data;
                    });
            }
        };

        // get company types
        service.getCompanyTypes = function () {
            if ($rootScope.companyTypes === undefined || $rootScope.companyTypes.length === 0) {
                $http({
                    method: 'GET',
                    url: api + '/values/getcompanytype?id',
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $rootScope.companyTypes = response.data;
                    });
            }
        };

        // get mode of transports
        service.getMots = function () {
            if ($rootScope.mots === undefined || $rootScope.mots.length === 0) {
                $http({
                    method: 'GET',
                    url: api + '/values/getmodeOftransport?id',
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $rootScope.mots = response.data;
                    });
            }
        };

        // get carriers
        service.getCarriers = function () {
            if ($rootScope.carriers === undefined || $rootScope.carriers.length === 0) {
                $http({
                    method: 'GET',
                    url: api + '/values/getcarrier?id',
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $rootScope.carriers = response.data;
                    });
            }
        };

        // get import export reason
        service.getImportExportReasons = function () {
            if ($rootScope.importExportReasons === undefined || $rootScope.importExportReasons.length === 0) {
                $http({
                    method: 'GET',
                    url: api + '/values/getimportexportreason',
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $rootScope.importExportReasons = response.data;
                    });
            }
        };

        // get IncoTerms
        service.getIncoTerms = function () {
            if ($rootScope.incoTerms === undefined || $rootScope.incoTerms.length === 0) {
                $http({
                    method: 'GET',
                    url: api + '/values/getincoterm?id',
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $rootScope.incoTerms = response.data;
                    });
            }
        };

        // get locations
        service.getLocations = function (countryid) {
            if ($rootScope.locations === undefined || $rootScope.locations.length === 0) {
                $http({
                    method: 'GET',
                    url: api + '/values/getlocations/' + countryid,
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $rootScope.locations = response.data;
                    });
            }
        };

        // get problems
        service.getProblems = function () {
            if ($rootScope.problems === undefined || $rootScope.problems.length === 0) {
                $http({
                    method: 'GET',
                    url: api + '/values/getproblem?id',
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $rootScope.problems = response.data;
                    });
            }
        };

        // get the statues
        service.loadStatuses = function () {
            if ($rootScope.statuses === undefined || $rootScope.statuses.length === 0) {
                $http({
                    method: 'GET',
                    url: api + '/values/getstatus?id',
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $rootScope.statuses = response.data;
                    });
            }
        };

        // get stuff modes
        service.getStuffModes = function () {
            if ($rootScope.stuffModes === undefined || $rootScope.stuffModes.length === 0) {
                $http({
                    method: 'GET',
                    url: api + '/values/getstuffmode?id',
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $rootScope.stuffModes = response.data;
                    });
            }
        };


        // validate a Company TIN is registered
        service.validateTin = function (tin, state) {
            // TODO: show a message window that explains the user needs to enter a valid TIN e.g.
            // Kindly enter a valid TIN that is registered on CargoCanal 
            // or leave the field blank if the Consignee's TIN is unavailable.
            // Also note that you could update this information later 
            // by using the 'Update Consignee Info.' option on an import/export document
            // To register a new consignee, 
            // 1. visit https://cargocanal2.com
            // 2. Click Register on the menu bar
            // 3. Enter all information requested. Make sure you have an active email account 
            //    as we would send your account activation information to the email provided.
            // 4. Visit the nearest CargoCanal Coorporate office to activate your account.
            //console.log(appFactory.getIsTinValid());
            if (tin === '' || tin === null || tin === undefined) {
                _isTinValid = true;
                if (state !== '') $state.go(state);
            }
            else {
                service.getCompanyByTin(tin)
                    .then(function (data) {
                        if (data === null || data === undefined) _isTinValid = false;
                        else _isTinValid = true;

                        if (_isTinValid && state !== '') $state.go(state);
                    });
            }
        };

        // get subscription types
        service.getSubscriptionTypes = function () {
            $http({
                method: 'GET',
                url: api + '/account/getsubscriptiontypes',
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    $rootScope.subscriptionTypes = response.data;
                }, function (error) {
                    $rootScope.subscriptionTypes = [];
                });
        };

        // get company subscriptions
        service.getCompanySubscriptions = function (id) {
            return $http({
                method: 'GET',
                url: api + '/account/getsubscriptionscurrent/' + id,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    return response.data;
                }, function (error) {
                    return [];
                });
        };

        // set company subscriptions
        service.setCompanySubscriptions = function (data) {
            return $http({
                method: 'POST',
                url: api + '/account/subscribe/',
                data: data,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    return response;
                }, function (error) { return null; });
        };

        // get if the Company TIN is valid
        service.getIsTinValid = function () {
            return _isTinValid;
        };

        // set Company TIN is valid
        service.setIsTinValid = function (bool) {
            _isTinValid = bool;
        };

        // get attachments
        service.getImportExportDocs = function (id) {
            return $http({
                method: 'GET',
                url: api + '/importexport/getimportexportdocs?id=' + id,
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            })
                .then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            //return $http({
            //    method: 'GET',
            //    url: api + '/values/validatetin?tin=' + tin,
            //    headers: { 'Content-Type': 'application/json; charset=utf-8' }
            //})
            //    .then(function (response) {
            //        return response.data;
            //    }, function (error) {
            //        return null;
            //    });
        };

        // get a Company object by exact TIN
        service.getCompanyByTin = function (tin) {
            return $http({
                method: 'GET',
                url: api + '/values/validatetin?tin=' + tin,
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            })
                .then(function (response) {
                    return response.data;
                }, function (error) {
                    return null;
                });
        };


        // gets the exchange rate for the day from OpenExchangeRates.org
        // and pass that to the object $rootScope.exRates
        service.getExRate = function () {
            return $.getJSON('https://openexchangerates.org/api/latest.json',
                { app_id: 'd6c5e9e7c538400b9c0ff657a1229810' },
                function (data) {
                    // loop through the rates to define the rates collection
                    $rootScope.exRates = {
                        base: data.base,
                        timestamp: data.timestamp,
                        rates: []
                    };

                    for (var prop in data.rates) {
                        $rootScope.exRates.rates.push({ Currency: prop, Rate: data.rates[prop] });
                    }
                    //$rootScope.exRates = data;
                });
        };


        // save costInfo
        service.saveCostInfo = function (cost) {
            var method = 'POST';
            var url = '/importexport/postcost';

            if (!isNaN(cost.ID) && cost.ID > 0) {
                method = 'PUT';
                url = '/importexport/putcost';
            }

            return $http({
                method: method,
                url: api + url,
                data: cost,
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            })
                .then(function (response) {
                    return response.data;
                }, function (error) { return null; });

        };

        // prep cards, dataTable
        service.prepCards = function () {
            setTimeout(function () {
                let elm = document.getElementById('cardsjs');
                if (elm !== undefined) elm.parentNode.removeChild(elm);

                let script = document.createElement('script');
                script.id = "cardsjs";
                script.src = "src/lib/cards.min.js";
                script.async = false; // This is required for synchronous execution
                document.body.appendChild(script);
            }, 1500);
        };

        // save LC
        service.saveLc = function (lc) {
            return $http({
                method: 'PUT',
                url: api + '/importexport/putlc',
                data: lc,
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            })
                .then(function (response) {
                    return response;
                },
                function (error) {
                    return null;
                });
        };

        // link TIN to import export
        service.linkTinToBill = function (consignee) {
            return $http({
                method: 'PUT',
                url: api + '/importexport/putconsigneetin',
                data: consignee,
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            })
                .then(function (response) {
                    return response.data;
                },
                function (error) {
                    return null;
                });
        };

        // get a Import
        service.getImports = function (skip, imports, searchText, odataParams) {
            if (odataParams === undefined || odataParams === null || odataParams === '')
                odataParams = '?$orderby=ID desc&$inlinecount=allpages&$expand=LC,Consignee,Items/ItemDetails,Cost';

            let fullUrl = '';
            let dataParams = {};

            switch (searchText) {
                case undefined: case '':
                    fullUrl = odataUrl + '/ODataImport(' + $rootScope.User.Company.ID + ')/GetImports' + odataParams;
                    dataParams = { 'skip': skip };
                    break;
                default:
                    fullUrl = odataUrl + '/ODataImport(' + $rootScope.User.Company.ID + ')/SearchImports' + odataParams;
                    dataParams = { 'skip': skip, 'searchText': searchText, 'token': $rootScope.User.Login.Token };
                    break;
            }

            return $http({
                method: 'POST',
                url: fullUrl,
                headers: { 'Content-Type': 'application/json; charset=utf-8' },
                data: dataParams
            })
                .then(function (response) {
                    return {
                        value: imports.concat(response.data.value),
                        odataInfo: {
                            'odata.metadata': response.data['odata.metadata'],
                            'odata.count': response.data['odata.count'],
                            'odata.nextLink': response.data['odata.nextLink']
                        }
                    };
                },
                function (error) {
                    return null;
                });
        };

        // get a Export
        service.getExports = function (skip, exports, searchText, odataParams) {
            if (odataParams === undefined || odataParams === null || odataParams === '')
                odataParams = '?$orderby=ID desc&$inlinecount=allpages&$expand=LC,Consignee,Items/ItemDetails,Cost';

            let fullUrl = '';
            let dataParams = {};

            switch (searchText) {
                case undefined: case '':
                    fullUrl = odataUrl + '/ODataExport(' + $rootScope.User.Company.ID + ')/GetExports' + odataParams;
                    dataParams = { 'skip': skip };
                    break;
                default:
                    fullUrl = odataUrl + '/ODataExport(' + $rootScope.User.Company.ID + ')/SearchExports' + odataParams;
                    dataParams = { 'skip': skip, 'searchText': searchText, 'token': $rootScope.User.Login.Token };
                    break;
            }

            return $http({
                method: 'POST',
                url: fullUrl,
                headers: { 'Content-Type': 'application/json; charset=utf-8' },
                data: dataParams
            })
                .then(function (response) {
                    return {
                        value: exports.concat(response.data.value),
                        odataInfo: {
                            'odata.metadata': response.data['odata.metadata'],
                            'odata.count': response.data['odata.count'],
                            'odata.nextLink': response.data['odata.nextLink']
                        }
                    };
                },
                function (error) {
                    return null;
                });
        };

        // gets import & exports. used by national bank and consignee views
        service.getImportExportByTinOrLc = function (model, searchText, searchButton, odataParams) {
            if (searchButton) model = []; // if user clicked the search button
            if (searchText === undefined) searchText = '';
            if (odataParams === undefined || odataParams === null || odataParams === '')
                odataParams = '?$orderby=ID desc&$inlinecount=allpages&$expand=LC,Consignee,Items/ItemDetails,Cost,StatusesViews';

            let fullUrl = odataUrl + '/ODataCustomer(' + $rootScope.User.Company.ID + ')/SearchImportExport' + odataParams;
            let dataParams = { 'skip': model.length, 'searchText': searchText, 'token': $rootScope.User.Login.Token };

            return $http({
                method: 'POST',
                url: fullUrl,
                headers: { 'Content-Type': 'application/json; charset=utf-8' },
                data: dataParams
            })
                .then(function (response) {

                    return {
                        value: model.concat(response.data.value),
                        odataInfo: {
                            'odata.metadata': response.data['odata.metadata'],
                            'odata.count': response.data['odata.count'],
                            'odata.nextLink': response.data['odata.nextLink']
                        }
                    };
                },
                function (error) {
                    return null;
                });
        };

        // get import based on search
        // _searchObject implies an object with definition {SearchText, Token, CompanyID}
        service.searchImport = function (text) {
            var _searchObject = {};
            _searchObject.Token = $rootScope.User.Login.Token;
            _searchObject.CompanyID = $rootScope.User.Company.ID;
            _searchObject.SearchText = text;


            return $http({
                method: 'POST',
                data: _searchObject,
                url: api + '/importexport/searchimports',
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            })
                .then(function (response) {
                    return response.data;
                }, function (error) {
                    return null;
                });
        };

        service.searchExport = function (text) {
            var _searchObject = {};
            _searchObject.Token = $rootScope.User.Login.Token;
            _searchObject.CompanyID = $rootScope.User.Company.ID;
            _searchObject.SearchText = text;


            return $http({
                method: 'POST',
                data: _searchObject,
                url: api + '/importexport/searchexports',
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            })
                .then(function (response) {
                    return response.data;
                }, function (error) {
                    return null;
                });

        };

        _initHelpers();
        return service;
    }]);

    app.factory('groupByFactory', ['$filter', function ($filter) {
        var _result = [];
        var service = {};
        service.groupByField = function (array, field) {
            if (array.length > 0) {
                _result = $filter('groupByField')(array, field);
            }

            return _result;
        };

        return service;
    }]);

    app.provider('datepickerProvider', [function () {
        // datepicker options
        this.dateOptions = {};

        // date formats
        this.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate', 'M!/d!/yyyy'];

        // pass this to dateDisabled property of datepickerProvider.dateoptions to disable weekends
        this.disableWeekends = function (data) {
            var date = data.date, mode = data.mode;
            return mode === 'day' && (date.getDay() === 0 || date.getDay() === 6);
        };

        // publicly available properties of this provider
        this.$get = function () {
            var that = this;
            return {
                getFormats: function () {
                    return that.formats;
                },
                getDateOptions: function () {
                    return that.dateOptions;
                },
                getDisableWeekends: function () {
                    return that.disableWeekends; // returns a function NOT FIRED
                }
            };
        };

    }]);


    // initialize the defaults
    app.config(['datepickerProviderProvider', function (datepickerProviderProvider) {
        // defaults datepicker options for datepickerProvider
        datepickerProviderProvider.dateOptions = {
            datepickerMode: 'day',
            formatYear: 'yyyy',
            showWeeks: false,
            dateDisabled: false,
            minDate: null,
            maxDate: new Date(),
            initDate: new Date()
        };
    }]);

})();