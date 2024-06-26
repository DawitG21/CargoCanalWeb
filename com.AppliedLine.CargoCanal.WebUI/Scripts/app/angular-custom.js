var app = angular.module('app', ['localization', 'ngAnimate', 'ui.router', 'ui.bootstrap', 'ngStorage', 'ngSanitize',
    'chartjsAngular', 'ngFileUpload', 'ngImgCrop']);

app.config(['$stateProvider', '$urlRouterProvider', '$locationProvider', function ($stateProvider, $urlRouterProvider, $locationProvider) {
    $locationProvider.hashPrefix(''); // remove annoying !
    $locationProvider.html5Mode(true); // pretty url with web.config set
    // catch all route
    // send users to the home page
    $urlRouterProvider.otherwise('/home');  

    $stateProvider.state('home', {
        url: '/home',
        templateUrl: 'views/home.html',
        controller: 'mainCtrl'
    });

    $stateProvider.state('passwordreset', {
        url: '/account/reset-password',
        templateUrl: 'views/admin/passwordreset.html',
        controller: 'passwordCtrl'
    });

    $stateProvider.state('passwordreset.changepassword', {
        url: '/change-password',
        templateUrl: 'views/admin/passwordreset_changepassword.html'
    });

    $stateProvider.state('termsofservice', {
        url: '/terms-of-service',
        templateUrl: 'views/terms_of_service.html'
    });

    $stateProvider.state('privacypolicy', {
        url: '/privacy-policy',
        templateUrl: 'views/privacy_policy.html'
    });

    $stateProvider.state('report', {
        url: '/report',
        templateUrl: 'views/reports/report.html',
        controller: 'reportController'
    });

    // TODO: Remodel Custom Report --> this uses the old templates and should be remodeled in the future
    $stateProvider.state('customreport', {
        url: '/customreport',
        templateUrl: 'templates_quarantined/report/dashboard-customreports.html',
        controller: 'reportCustomCtrl'
    });

    $stateProvider.state('report.cargo_dispatched_weight_grouped_by_month_report', {
        url: '/001',
        templateUrl: 'views/reports/cargo_dispatched_weight_grouped_by_month.html'
    });

    $stateProvider.state('report.cargo_import_weight_grouped_by_tin_report', {
        url: '/002',
        templateUrl: 'views/reports/cargo_import_weight_grouped_by_tin.html'
    });

    $stateProvider.state('report.cargo_on_voyage_only_grouped_by_country_report', {
        url: '/003',
        templateUrl: 'views/reports/cargo_on_voyage_only_grouped_by_country.html'
    });

    $stateProvider.state('report.problem_grouped_by_tin_report', {
        url: '/004',
        templateUrl: 'views/reports/problem_grouped_by_tin.html'
    });

    $stateProvider.state('report.problem_grouped_by_tin_unresolved_report', {
        url: '/005',
        templateUrl: 'views/reports/problem_grouped_by_tin_unresolved.html'
    });

    $stateProvider.state('report.demurrage_grouped_by_tin_report', {
        url: '/006',
        templateUrl: 'views/reports/demurrage_grouped_by_tin.html'
    });

    $stateProvider.state('report.demurrage_grouped_by_tin_active_report', {
        url: '/007',
        templateUrl: 'views/reports/demurrage_grouped_by_tin_active.html'
    });

    $stateProvider.state('report.transit_time_grouped_by_import_report', {
        url: '/008',
        templateUrl: 'views/reports/transit_time_grouped_by_import.html'
    });

    $stateProvider.state('report.transit_time_after_discharge_grouped_by_import_report', {
        url: '/008-1',
        templateUrl: 'views/reports/transit_time_after_discharge_grouped_by_import.html'
    });

    $stateProvider.state('report.transit_time_after_discharge_grouped_by_discharge_port_report', {
        url: '/008-2',
        templateUrl: 'views/reports/transit_time_after_discharge_grouped_by_discharge_port.html'
    });

    $stateProvider.state('report.transit_time_grouped_by_country_report', {
        url: '/009',
        templateUrl: 'views/reports/transit_time_grouped_by_country.html'
    });

    $stateProvider.state('report.transit_time_grouped_by_country_summary_report', {
        url: '/010',
        templateUrl: 'views/reports/transit_time_grouped_by_country_summary.html'
    });

    $stateProvider.state('report.transit_time_grouped_by_tin_report', {
        url: '/011',
        templateUrl: 'views/reports/transit_time_grouped_by_tin.html'
    });

    $stateProvider.state('report.bill_statuses_grouped_by_tin_report', {
        url: '/012',
        templateUrl: 'views/reports/bill_statuses_grouped_by_tin.html'
    });

    $stateProvider.state('dashboard', {
        url: '/dashboard',
        templateUrl: 'views/dashboard.html',
        controller: 'dashboardController'
    });

    $stateProvider.state('account', {
        url: '/account',
        templateUrl: 'views/account.html',
        controller: 'accountCtrl'
    });

    $stateProvider.state('account.support', {
        url: '/support',
        templateUrl: 'views/support.html',
        controller: 'supportCtrl'
    });

    $stateProvider.state('account.uploadprofileimage', {
        url: '/upi',
        templateUrl: 'views/account_upload_profile_image.html'
    });

    $stateProvider.state('account.subscriptions', {
        url: '/subscriptions',
        templateUrl: 'views/account_subscriptions.html'
    });

    $stateProvider.state('account.users', {
        url: '/users',
        templateUrl: 'views/account_users.html',
        controller: 'adminCtrl'
    });

    $stateProvider.state('account.company', {
        url: '/company',
        templateUrl: 'views/account_company.html',
        controller: 'adminCtrl'
    });

    $stateProvider.state('account.company.uploadcompanylogo', {
        url: '/uclogo',
        templateUrl: 'views/account_upload_profile_image.html'
    });

    $stateProvider.state('login', {
        url: '/login',
        templateUrl: 'views/login.html',
        controller: 'loginController'
    });

    $stateProvider.state('register', {
        url: '/register',
        templateUrl: 'views/admin/create_company.html',
        controller: 'createConsigneeCtrl'
    });

    $stateProvider.state('maritime', {
        url: '/mt',
        templateUrl: 'views/admin/maritime.html',
        controller: 'maritimeController'
    });

    $stateProvider.state('maritime.import', {
        url: '/import',
        templateUrl: 'views/import/import_maritime.html',
        controller: 'importController'
    });

    $stateProvider.state('maritime.export', {
        url: '/export',
        templateUrl: 'views/export/export_maritime.html',
        controller: 'exportController'
    });

    $stateProvider.state('maritime.createcompany', {
        url: '/create/company',
        templateUrl: 'views/admin/create_company.html'
    });

    $stateProvider.state('import', {
        url: '/import',
        templateUrl: 'views/import/import.html',
        controller: 'importController'
    });

    $stateProvider.state('import.add', {
        url: '/a',
        templateUrl: 'views/import/add_consignee.html'
    });

    $stateProvider.state('import.add.bill', {
        url: '/bill',
        templateUrl: 'views/import/add_bill.html'
    });

    $stateProvider.state('import.add.items', {
        url: '/items',
        templateUrl: 'views/import/add_items.html'
    });

    $stateProvider.state('import.add.carrier', {
        url: '/carrier',
        templateUrl: 'views/import/add_carrier.html'
    });

    $stateProvider.state('export', {
        url: '/export',
        templateUrl: 'views/export/export.html',
        controller: 'exportController'
    });

    $stateProvider.state('export.add', {
        url: '/a',
        templateUrl: 'views/export/add_consignee.html'
    });

    $stateProvider.state('export.add.bill', {
        url: '/bill',
        templateUrl: 'views/export/add_bill.html'
    });

    $stateProvider.state('export.add.bill2', {
        url: '/stuffloc',
        templateUrl: 'views/export/add_bill2.html'
    });

    $stateProvider.state('export.add.items', {
        url: '/items',
        templateUrl: 'views/export/add_items.html'
    });

    $stateProvider.state('export.add.carrier', {
        url: '/carrier',
        templateUrl: 'views/export/add_carrier.html'
    });

    // Maritime Daily Activity Specific Functions
 
    $stateProvider.state('breakbulk', {
        url: '/breakbulk',
        templateUrl: 'views/activitybreakbulk/break_bulk_activity.html',
        controller: 'indexBreakBulkCtrl'
    });

    $stateProvider.state('breakbulk.create', {
        url: '/create',
        templateUrl: 'views/activitybreakbulk/create_breakbulk.html',
        controller: 'indexBreakBulkCtrl'
    });

    $stateProvider.state('breakbulk.edit', {
        url: '/edit',
        templateUrl: 'views/activitybreakbulk/edit_breakbulk.html',
        controller: 'indexBreakBulkCtrl'
    });

    ///
    $stateProvider.state('multimodal', {
        url: '/multimodal',
        templateUrl: 'views/activitymultimodal/multi_modal_activity.html',
        controller: 'indexMultiModalCtrl'
    });

    $stateProvider.state('multimodal.create', {
        url: '/create',
        templateUrl: 'views/activitymultimodal/create_multimodal.html',
        controller: 'indexMultiModalCtrl'
    });

    $stateProvider.state('multimodal.edit', {
        url: '/edit',
        templateUrl: 'views/activitymultimodal/edit_multimodal.html',
        controller: 'indexMultiModalCtrl'
    });

    ///
    $stateProvider.state('unimodal', {
        url: '/unimodal',
        templateUrl: 'views/activityunimodal/uni_modal_activity.html',
        controller: 'indexUniModalCtrl'
    });

    $stateProvider.state('unimodal.create', {
        url: '/create',
        templateUrl: 'views/activityunimodal/create_unimodal.html',
        controller: 'indexUniModalCtrl'
    });

    $stateProvider.state('unimodal.edit', {
        url: '/edit',
        templateUrl: 'views/activityunimodal/edit_unimodal.html',
        controller: 'indexUniModalCtrl'
    });

    ///
    $stateProvider.state('djiboutifreezone', {
        url: '/djiboutifreezone',
        templateUrl: 'views/activityfreezone/free_zone_activity.html',
        controller: 'indexfreeZoneCtrl'
    });

    $stateProvider.state('djiboutifreezone.create', {
        url: '/create',
        templateUrl: 'views/activityfreezone/create_freezone.html',
        controller: 'createFreeZoneCtrl'
    });

    $stateProvider.state('djiboutifreezone.edit', {
        url: '/edit',
        templateUrl: 'views/activityfreezone/edit_freezone.html',
        controller: 'editFreeZoneCtrl'
    });

    ///
    $stateProvider.state('drynonepacked', {
        url: '/drynonepacked',
        templateUrl: 'views/activitynonepacked/none_packed_activity.html',
        controller: 'indexnonePackedCtrl'
    });

    $stateProvider.state('drynonepacked.create', {
        url: '/create',
        templateUrl: 'views/activitynonepacked/create_nonepacked.html',
        controller: 'createNonePackedCtrl'
    });

    $stateProvider.state('drynonepacked.edit', {
        url: '/edit',
        templateUrl: 'views/activitynonepacked/edit_nonepacked.html',
        controller: 'editNonePackedCtrl'
    });

    ///
    $stateProvider.state('oiltransport', {
        url: '/oiltransport',
        templateUrl: 'views/activityoiltransport/oil_transport_activity.html',
        controller: 'indexoilTransportCtrl'
    });

    $stateProvider.state('oiltransport.create', {
        url: '/create',
        templateUrl: 'views/activityoiltransport/create_oiltransport.html',
        controller: 'createOilTransportCtrl'
    });

    $stateProvider.state('oiltransport.edit', {
        url: '/edit',
        templateUrl: 'views/activityoiltransport/edit_oiltransport.html',
        controller: 'editOilTransportCtrl'
    });

    ///
    $stateProvider.state('djiboutitajura', {
        url: '/djiboutitajura',
        templateUrl: 'views/admin/djibouti_tajura_activity.html',
        controller: 'indexDjiboutiTajuraCtrl'
    });

    $stateProvider.state('djiboutitajura.create', {
        url: '/create',
        templateUrl: 'views/activitydjiboutitajura/create_djibouti_tajura.html',
        controller: 'createDjiboutiTajuraCtrl'
    });

    $stateProvider.state('djiboutitajura.edit', {
        url: '/edit',
        templateUrl: 'views/activitydjiboutitajura/edit_djibouti_tajura.html',
        controller: 'editDjiboutiTajuraCtrl'
    });
}]);
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

    app.factory('appFactory', ['$rootScope', '$sessionStorage', '$timeout', '$state', '$http', '$q', '$sce', function ($rootScope, $sessionStorage, $timeout, $state, $http, $q, $sce) {
        $rootScope.modalOpen = false;
        var _isTinValid = true;
        var service = {};
        var odataUrl = api.substring(0, api.indexOf('/api')) + '/odata';
        var date = new Date();
        
        service.setModalOpen = function (bool) {
            $rootScope.modalOpen = bool;
        };

        // show the loading animation
        service.showLoader = (message) => {
            $rootScope.loader = true;
            $rootScope.loaderMessage = !message ? 'processing...' : message;
        };

        // close the loading animation
        service.closeLoader = () => {
            $rootScope.loader = false;
            $rootScope.loaderMessage = '';
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
        };

        // initialize common functions used by multiple controllers
        let _initHelpers = function () {
            $rootScope.getPhotoUrl = service.getPhotoUrl;

            // logout of the app, go to home
            // clear all saved cookies, sessions, rootscope
            $rootScope.logout = function () {
                // cancle the timeout operation
                $timeout.cancel($rootScope.timeOutSession);

                $sessionStorage.__user = $rootScope.User = undefined;
                $state.go('home');
            };

            $rootScope.scrollToTop = function () {
                document.body.scrollTop = 0;
                document.documentElement.scrollTop = 0;
            };

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
                        // console.log('Ports service', $rootScope.pols[0]);
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
            // 1. visit https://cargocanal.com
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

        // get company subscription alert
        service.showSubscriptionAlert = () => {
            if ($rootScope.User.Company.Subscription) {
                let today = (new Date()).getTime();

                let subscriptionExpiryDate = $rootScope.User.Company.Subscription[0].ExpiryDate;
                let subscriptionExpiryDays = Math.floor(((new Date(subscriptionExpiryDate)).getTime() - today) / (24 * 3600 * 1000));

                $rootScope.subscriptionExpiryDays = subscriptionExpiryDays;

                if (subscriptionExpiryDays <= 30) {
                    $('#modalSubscriptionAlert').modal('show');
                }
            }
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

        // get breakBulkActivities       
        service.getBreakBulkActivities = function (skip, breakBulkActivities, searchText, odataParams) {
            if (odataParams === undefined || odataParams === null || odataParams === '')
                odataParams = '?$orderby=ID desc&$inlinecount=allpages';                
                //fromDate = date | 'yyyy-MM-dd';
                //toDate = fromDate;

            let fullUrl = '';
            let dataParams = {};

            switch (searchText) {
                case undefined: case '':
                    fullUrl = odataUrl + '/ODataMaritimeBreakBulk(' + $rootScope.User.Company.ID + ')/SearchDailyBreakBulk' + odataParams;
                   // console.log(fullUrl);
                    dataParams = { 'skip': skip };
                    //dataParams = { 'skip': skip, 'searchText': searchText, 'fromDate': fromDate, 'toDate': toDate };
                    break;
                default:
                    fullUrl = odataUrl + '/ODataMaritimeBreakBulk(' + $rootScope.User.Company.ID + ')/SearchDailyBreakBulk' + odataParams;
                  //  console.log(fullUrl);
                    dataParams = { 'skip': skip, 'searchText': searchText, 'token': $rootScope.User.Login.Token };
                   // dataParams = { 'skip': skip, 'searchText': searchText, 'fromDate': fromDate, 'toDate': toDate, 'token': $rootScope.User.Login.Token };
                    break;
            }

            return $http({
                method: 'POST',
                url: fullUrl,
                headers: { 'Content-Type': 'application/json; charset=utf-8' },
                data: dataParams
            })
                .then(function (response) {                 
                  // console.log('response',response.data.value);
                    return {
                        value: breakBulkActivities.concat(response.data.value),                       
                        odataInfo: {
                            'odata.metadata': response.data['odata.metadata'],
                            'odata.count': response.data['odata.count'],
                            'odata.nextLink': response.data['odata.nextLink']
                        }                         
                    };                   
                },
                function (error) {
                    console.log('ERROR', error);
                        return null;
                    });
        };

        // get multiModalActivities       
        service.getMultiModalActivities = function (skip, multiModalActivities, searchText, odataParams) {
            if (odataParams === undefined || odataParams === null || odataParams === '')
                odataParams = '?$orderby=ID desc&$inlinecount=allpages';
            //fromDate = date | 'yyyy-MM-dd';
            //toDate = fromDate;

            let fullUrl = '';
            let dataParams = {};

            switch (searchText) {
                case undefined: case '':
                    fullUrl = odataUrl + '/ODataMaritimeMultiModalTransport(' + $rootScope.User.Company.ID + ')/SearchDailyMultiModalTransport' + odataParams;
                   // console.log('url:', fullUrl);
                    dataParams = { 'skip': skip };                  
                    break;
                default:
                    fullUrl = odataUrl + '/ODataMaritimeMultiModalTransport(' + $rootScope.User.Company.ID + ')/SearchDailyMultiModalTransport' + odataParams;
                   // console.log('url_:', fullUrl);
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
                   // console.log('response',response.data.value);
                    return {
                        value: multiModalActivities.concat(response.data.value),
                        odataInfo: {
                            'odata.metadata': response.data['odata.metadata'],
                            'odata.count': response.data['odata.count'],
                            'odata.nextLink': response.data['odata.nextLink']
                        }
                    };
                },
                    function (error) {
                        console.log('ERROR', error);
                        return null;
                    });
        };

        // get uniModalActivities       
        service.getUniModalActivities = function (skip, uniModalActivities, searchText, odataParams) {
            if (odataParams === undefined || odataParams === null || odataParams === '')
                odataParams = '?$orderby=ID desc&$inlinecount=allpages';
            //fromDate = date | 'yyyy-MM-dd';
            //toDate = fromDate;

            let fullUrl = '';
            let dataParams = {};

            switch (searchText) {
                case undefined: case '':
                    fullUrl = odataUrl + '/ODataMaritimeUniModalTransport(' + $rootScope.User.Company.ID + ')/SearchDailyUniModalTransport' + odataParams;
                    //console.log(fullUrl);
                    dataParams = { 'skip': skip };

                    break;
                default:
                    fullUrl = odataUrl + '/ODataMaritimeUniModalTransport(' + $rootScope.User.Company.ID + ')/SearchDailyUniModalTransport' + odataParams;
                    //  console.log(fullUrl);
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
                    //console.log('response',response.data.value);
                    return {
                        value: uniModalActivities.concat(response.data.value),
                        odataInfo: {
                            'odata.metadata': response.data['odata.metadata'],
                            'odata.count': response.data['odata.count'],
                            'odata.nextLink': response.data['odata.nextLink']
                        }
                    };
                },
                    function (error) {
                        //   console.log('ERROR', error);
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
(function () {
    'use strict';


    app.service('appAnalytics', ['$rootScope', '$http', function ($rootScope, $http) {
        let _helperTopCountries = function (url) {
            return $http({
                method: 'POST',
                url: api + url,
                data: { Token: $rootScope.User.Login.Token },
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    return response.data;
                }, function (error) {
                    throw new Error(error);
                });
        };

        // Service Analytics for imports
        this.topImportCountries = function () {
            return _helperTopCountries('/importexport/topimportcountries');
        };

        // Service Analytics for exports
        this.topExportCountries = function () {
            return _helperTopCountries('/importexport/topexportcountries');
        };
    }]);
})();
(function () {
    'use strict';


    app.service('commentService', ['$rootScope', '$http', function ($rootScope, $http) {
        let _url = '/comment';


        this.getComments = function (id) {
            return $http({
                method: 'GET',
                url: api + _url + '/getcomment/' + id,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    return response.data;
                }, function (error) {
                    throw new Error(error);
                });
        };

        // post a comment
        this.submitComment = function (comment) {
            // assign token
            comment.Token = $rootScope.User.Login.Token;

            return $http({
                method: 'POST',
                url: api + _url,
                data: comment,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    return response.data;
                }, function (error) {
                    throw new Error(error);
                });
        };

    }]);
})();
(function () {
    'use strict';
    app.factory('signalRHubProxy', ['$rootScope', function ($rootScope) {
        function signalRHubProxyFactory(serverUrl, hubName) {
            var _connection = $.hubConnection(serverUrl);
            var _hubProxy = _connection.createHubProxy(hubName);
            //_connection.start()
            //    .done(function () { console.log('connected id: ' + _connection.id); })
            //    .fail(function () { console.log('could not connect'); });
            //connection.start({ withCredentials: false }).done(function () { });

            return {
                on: function (eventName, callback) {
                    _hubProxy.on(eventName, function (result) {
                        var args = arguments;
                        $rootScope.$apply(function () {
                            callback.apply(_hubProxy, args);
                        });
                    });

                    _connection.start()
                        .done(function () { console.log('connected signal'); }) // + _connection.id
                        .fail(function () { console.log('failed signal'); });
                },
                off: function (eventName, callback) {
                    _hubProxy.off(eventName, function (result) {
                        var argsOff = arguments;
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback.apply(_hubProxy, argsOff);
                            }
                        });
                    });
                }
            };
        }

        return signalRHubProxyFactory;
    }]);
})();
(function () {
    'use strict';

    app.factory('sessionTimeoutFactory', ['$interval', '$timeout', '$document', '$rootScope', function ($interval, $timeout, $document, $rootScope) {
        var factory = {};

        let handlerModal;

        // initializations 
        const timeOutValue = 600000; // idle time of 10 minutes
        $rootScope.maxSessionCounter = 20;// idle grace period
        $rootScope.displaySessionModal = false;


        const countdown = function () {
            $rootScope.counter = $rootScope.counter - 1;
        };


        const resetCounter = function () {
            $rootScope.counter = angular.copy($rootScope.maxSessionCounter); 
        };


        const showModal = function () {
            $rootScope.$apply(() => {
                $rootScope.displaySessionModal = true;
            });
        };


        const hideModal = function () {
            // $timeout to schedule scope in future stack 
            // and ensure code is called in single $apply block

            $timeout(() => {
                $rootScope.displaySessionModal = false;
            }, 0);
        };


        const clearModalInterval = function () {
            $interval.cancel(handlerModal);
        };


        const logOut = function () {
            hideModal();
            $rootScope.logout();
        };


        const updateCounter = function () {
            if ($rootScope.counter < 0) return;

            countdown();

            if ($rootScope.counter <= 0) {
                // clear modal handler and logout
                clearModalInterval();
                logOut();
            }
        };

        // show modal with countdown
        // attach handler
        const modalCountdown = function () {
            resetCounter();

            handlerModal = $interval(function () {
                updateCounter();
            }, 1000);
        };


        const startCountdown = function () {
            showModal();
            modalCountdown();
        };



        // used to initialize timeout countdown
        factory.timeoutInit = function () {
            $timeout.cancel($rootScope.timeOutSession);

            $rootScope.timeOutSession = $timeout(function () {
                startCountdown(); // initiates the countdown display
            }, timeOutValue);
        };


        function cancelTimeOut(e) {
            if ($rootScope.User) {
                hideModal();

                // clear handlerModal
                clearModalInterval();

                // invoke timeout listener if user is still connected
                factory.timeoutInit();
            }
        }


        var bodyElement = angular.element($document);
        angular.forEach(['keydown', 'keyup', 'mousemove', 'click', 'DOMMouseScroll', 'mousewheel', 'mousedown',
            'touchstart', 'touchmove', 'scroll', 'focus'
        ], function (eventName) {
            bodyElement.bind(eventName, function (e) {
                cancelTimeOut(e);
            });
        });


        return factory;
    }]);

})();
//var serverUrl = 'http://localhost:52931';
var serverUrl = 'https://appliedline.com/cargocanalapi';
var api = serverUrl + '/api';

(function () {
    'use strict';

    app.controller('mainCtrl', ['$scope', '$rootScope', '$state', '$http', '$sessionStorage', 'localize', 'refresher', 'appFactory', 'passwordFactory', 'signalRHubProxy', 'datepickerProvider',
        function ($scope, $rootScope, $state, $http, $sessionStorage, localize, refresher, appFactory, passwordFactory, signalRHubProxy, datepickerProvider) {
            //fixes angular refresh page (all local variables lose data) issue
            refresher.refreshApp();

            // initialize the dateOptions
            $scope.dateOptions = datepickerProvider.getDateOptions();

            $scope.required = false;
            $scope.showInput = false;
            $scope.visible = true;

            $scope.today = function () {
                $scope.dt = new Date();
            };

            // clears the date
            $scope.clear = function () {
                $scope.dt = null;
            };

            // dateOptions with a class illustration
            $scope.inlineOptions = {
                customClass: getDayClass,
                minDate: new Date(),
                showWeeks: true
            };

            $scope.toggleMin = function () {
                $scope.inlineOptions.minDate = !$scope.inlineOptions.minDate ? null : new Date();
                $scope.dateOptions.minDate = $scope.inlineOptions.minDate;
            };

            $scope.openDt = function () {
                $scope.popupDt.opened = true;
                $scope.showInput = true;
                $scope.visible = false;
            };

            $scope.popupDt = {
                opened: false
            };

            $scope.setDate = function (year, month, day) {
                $scope.dt = new Date(year, month, day);
            };

            // takes string date to set $scope.dt and returns the value
            $scope.setDate2 = function (dt) {
                return $scope.dt = new Date(dt);
            };

            $scope.formats = datepickerProvider.getFormats();
            $scope.format = $scope.formats[0];
            $scope.altInputFormats = datepickerProvider.getFormats();

            var tomorrow = new Date();
            tomorrow.setDate(tomorrow.getDate() + 1);
            var afterTomorrow = new Date();
            afterTomorrow.setDate(tomorrow.getDate() + 1);
            $scope.events = [
                {
                    date: tomorrow,
                    status: 'full'
                },
                {
                    date: afterTomorrow,
                    status: 'partially'
                }
            ];

            function getDayClass(data) {
                var date = data.date,
                    mode = data.mode;
                if (mode === 'day') {
                    var dayToCheck = new Date(date).setHours(0, 0, 0, 0);

                    for (var i = 0; i < $scope.events.length; i++) {
                        var currentDay = new Date($scope.events[i].date).setHours(0, 0, 0, 0);

                        if (dayToCheck === currentDay) {
                            return $scope.events[i].status;
                        }
                    }
                }

                return '';
            }

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
                    default:
                        return 'common-sprite container-32';
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

    app.controller('maritimeController', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            // Maritime Specific Functions
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

            $scope.activitylist = [
                {
                    "id": "breakbulk",
                    "name": "break bulk"
                },
                {
                    "id": "multimodal",
                    "name": "multi modal"
                },
                {
                    "id": "unimodal",
                    "name": "uni modal"
                },
                //{
                //    "id": "djiboutifreezone",
                //    "name": "djibouti freezone"
                //},
                //{
                //    "id": "drynonepacked",
                //    "name": "dry (none packed) transport"
                //},
                //{
                //    "id": "djiboutitajura",
                //    "name": "djibouti & tajura import corridor"
                //},
                //{
                //    "id": "oiltransport",
                //    "name": "oil transport"
                //},
            ];


            $scope.goToState = function (selectedActivity) {
                if (!selectedActivity) return;
                $state.go(selectedActivity);
            };

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
(function () {
    'use strict';

    app.controller('loginController', ["$scope", "$http", 'sessionTimeoutFactory', "$sessionStorage", "$rootScope", "$state", "appFactory",
        function ($scope, $http, sessionTimeoutFactory, $sessionStorage, $rootScope, $state, appFactory) {
            $scope.login = {};
            $scope.processing = false;

            $rootScope.scrollToTop();

            $scope.forgotPassword = function () {
                $state.go('passwordreset');
            };

            $scope.signIn = function () {
                $scope.loginFailed = '';
                $scope.processing = true;

                appFactory.showLoader('verifying credentials...');

                $http({
                    method: 'POST',
                    url: api + '/account/postlogin',
                    data: $scope.login,
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        appFactory.closeLoader();

                        // invoke timeout
                        sessionTimeoutFactory.timeoutInit();

                        // get the user collection and save it in session
                        $sessionStorage.__user = $rootScope.User = response.data;

                        // get user subscriptions
                        appFactory.getCompanySubscriptions($rootScope.User.Company.ID)
                            .then(function (data) {
                                $rootScope.User.Company.Subscription = data;
                                // get the user collection and save it in session
                                $sessionStorage.__user = $rootScope.User;
                                appFactory.showSubscriptionAlert();
                            });


                        // set user profile and company profile pictures to base64
                        appFactory.setDataImage();
                        if ($rootScope.User.Login.LastSeen === null | undefined) {
                            $state.go('account');
                        }
                        else $state.go('dashboard');

                        // activate session validation
                        // $rootScope.workerValidateSession();
                    }, function (error) {
                        appFactory.closeLoader();
                        $scope.processing = false;

                        if (error.status === -1) {
                            $rootScope.noInternet();
                        } else {
                            $scope.loginFailed = "Invalid login attempt.";
                        }
                    });
            };
        }]);
})();
(function () {
    'use strict';

    app.controller('dashboardController', ['$scope', '$filter', '$http', '$state', '$rootScope', '$sessionStorage', '$timeout', 'chartjsFactory', 'appFactory', 'appAnalytics',
        function ($scope, $filter, $http, $state, $rootScope, $sessionStorage, $timeout, chartjsFactory, appFactory, appAnalytics) {
            // go to home, if the user is not logged in
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');

            // TODO: Subscription Alert
            // show subscription
            appFactory.showSubscriptionAlert();

            var shipmentCanvas, shipmentChart;
            var forwarderActivityCanvas, forwarderActivityChart;
            var demurrageChart, demurrageCanvas;
            const suggestedMax = 5; // maximum step size

            $scope.daysOfShipmentActivity = $rootScope.days[2].value;
            $scope.daysOfDemurrageActivity = $rootScope.days[2].value;
            $scope.daysOfForwarderActivity = $rootScope.days[2].value;


            $scope.dashboard = {
                import: {
                    data: {},
                    // get the dashboard import summary for the company
                    get: function () {
                        // send user token which the server would use to process dashboard data
                        $http({
                            method: 'POST',
                            url: api + '/importexport/dashboardimportsummary',
                            data: $rootScope.User.Login,
                            headers: { 'Content-Type': 'application/json; charset=utf-8' }
                        })
                            .then(function (response) {
                                $scope.dashboard.import.data = response.data;
                                if (response.data === null) {
                                    $scope.dashboard.import.data = {};
                                    $scope.dashboard.import.data.Total = 0;
                                    $scope.dashboard.import.data.PercentPending = 0;
                                    $scope.dashboard.import.data.PercentCompleted = 0;
                                }
                            });
                    }
                },
                export: {
                    data: {},
                    // get the dashboard export summary for the company
                    get: function () {
                        // send user token which the server would use to process dashboard data
                        $http({
                            method: 'POST',
                            url: api + '/importexport/dashboardexportsummary',
                            data: $rootScope.User.Login,
                            headers: { 'Content-Type': 'application/json; charset=utf-8' }
                        })
                            .then(function (response) {
                                $scope.dashboard.export.data = response.data;
                                if (response.data === null | undefined) {
                                    $scope.dashboard.export.data = {};
                                    $scope.dashboard.export.data.Total = 0;
                                    $scope.dashboard.export.data.PercentPending = 0;
                                    $scope.dashboard.export.data.PercentCompleted = 0;
                                }
                            });
                    }
                },
                shipments: {
                    data: {},
                    get: function (data) {
                        // TODO: fetch only once script is done loading
                        // send user token which the server would use to process dashboard data
                        let max = 0;
                        $scope.daysOfShipmentActivity = data || $scope.daysOfShipmentActivity;
                        $scope.loadingShipments = true;

                        // clear the existing chart
                        if (shipmentChart) {
                            shipmentChart.destroy();
                        }

                        // wait a few seconds
                        $timeout(() => {

                            $http({
                                method: 'POST',
                                url: api + '/importexport/dashboardshipmentsanalytics',
                                data: { Token: $rootScope.User.Login.Token, Days: $scope.daysOfShipmentActivity },
                                headers: { 'Content-Type': 'application/json; charset=utf-8' }
                            })
                                .then(function (response) {
                                    // no data to draw Chart
                                    if (response.data === null | undefined
                                        || response.data.labels.length === 0) return;

                                    $scope.dashboard.shipments.data = response.data;
                                    $scope.chartOptions = {
                                        backgroundColors: [
                                            'rgba(62, 84, 121, 0.1)',
                                            'rgba(255, 10, 182, 0.1)'
                                        ],
                                        borderColors: [
                                            'rgba(62, 84, 121, 1)',
                                            'rgba(255, 10, 182, 1)'
                                        ]
                                    };

                                    $scope.dashboard.shipments.data.datasets = [];

                                    for (let i in $scope.dashboard.shipments.data.data) {
                                        $scope.dashboard.shipments.data.datasets.push({
                                            data: angular.copy($scope.dashboard.shipments.data.data[i]),
                                            label: $scope.dashboard.shipments.data.series[i]
                                        });
                                    }

                                    $scope.dashboard.shipments.data.data = undefined;
                                    $scope.dashboard.shipments.data.series = undefined;

                                    // filter the date part of the data labels
                                    for (let i in $scope.dashboard.shipments.data.labels) {
                                        $scope.dashboard.shipments.data.labels[i] = $filter('date')($scope.dashboard.shipments.data.labels[i], 'MMM dd');
                                    }

                                    for (let i in $scope.dashboard.shipments.data.datasets) {
                                        let mod = i % $scope.chartOptions.borderColors.length;
                                        $scope.dashboard.shipments.data.datasets[i].backgroundColor = $scope.chartOptions.backgroundColors[mod];
                                        $scope.dashboard.shipments.data.datasets[i].borderColor = $scope.chartOptions.borderColors[mod];
                                        $scope.dashboard.shipments.data.datasets[i].lineTension = 0;

                                        let o = $scope.dashboard.shipments.data.datasets[i].data;
                                        for (let j in o) {
                                            if (o[j] !== null && o[j] > max) {
                                                max = o[j];
                                            }
                                        }
                                    }

                                    // clear the existing chart
                                    if (shipmentChart) {
                                        shipmentChart.destroy();
                                    }

                                    shipmentCanvas = chartjsFactory.setCanvas('canvas1');
                                    shipmentChart = chartjsFactory.createChart('line',
                                        $scope.dashboard.shipments.data,
                                        {
                                            scales: {
                                                xAxes: [{
                                                    scaleLabel: {
                                                        display: true,
                                                        labelString: 'Date Inserted'
                                                    }
                                                }],
                                                yAxes: [{
                                                    scaleLabel: {
                                                        display: true,
                                                        labelString: 'No. of Cargo'
                                                    },
                                                    ticks: {
                                                        beginAtZero: true,
                                                        stepSize: max > suggestedMax ? Math.ceil(max / suggestedMax) : 1,
                                                        suggestedMax: suggestedMax
                                                    }
                                                }]
                                            }
                                        });
                                    $scope.loadingShipments = false;
                                }, (error) => { $scope.loadingShipments = false; });

                        }, 5000);
                    }
                },
                forwarderActivity: {
                    data: {},
                    get: function (data) {
                        // TODO: fetch only once script is done loading
                        // send user token which the server would use to process dashboard data
                        let max = 0;
                        $scope.daysOfForwarderActivity = data || $scope.daysOfForwarderActivity;
                        $scope.loadingForwarderActivity = true;

                        // clear the existing chart
                        if (forwarderActivityChart) {
                            forwarderActivityChart.destroy();
                        }

                        // wait a few seconds
                        $timeout(() => {

                            $http({
                                method: 'POST',
                                url: api + '/importexport/dashboardforwardersactivityanalytics',
                                data: { Token: $rootScope.User.Login.Token, Days: $scope.daysOfForwarderActivity },
                                headers: { 'Content-Type': 'application/json; charset=utf-8' }
                            })
                                .then(function (response) {
                                    // no data to draw Chart
                                    if (response.data === null | undefined
                                        || response.data.labels.length === 0) return;

                                    $scope.dashboard.forwarderActivity.data = response.data;
                                    $scope.chartOptions = {
                                        backgroundColors: [
                                            'rgba(0, 255, 0, 1)',
                                            'rgba(255, 10, 182, 1)'
                                        ],
                                        borderColors: [
                                            'rgba(62, 84, 121, 1)',
                                            'rgba(255, 10, 182, 1)'
                                        ]
                                    };

                                    $scope.dashboard.forwarderActivity.data.datasets = [{ data: [], backgroundColor: []}];

                                    for (let i in $scope.dashboard.forwarderActivity.data.data) {
                                        $scope.dashboard.forwarderActivity.data.datasets[0].data.push(
                                            $scope.dashboard.forwarderActivity.data.data[i].length
                                        );

                                        let mod = i % $scope.chartOptions.borderColors.length;
                                        $scope.dashboard.forwarderActivity.data.datasets[0].backgroundColor.push($scope.chartOptions.backgroundColors[i]);
                                    }


                                    // clear the existing chart
                                    if (forwarderActivityChart) {
                                        forwarderActivityChart.destroy();
                                    }

                                    forwarderActivityCanvas = chartjsFactory.setCanvas('canvas_forwarder_activity');
                                    forwarderActivityChart = chartjsFactory.createChart('doughnut',
                                        $scope.dashboard.forwarderActivity.data,
                                        {
                                            //layout: {
                                            //    padding: {
                                            //        left: 150,
                                            //        right: 150,
                                            //        top: 0,
                                            //        bottom: 0
                                            //    }
                                            //}
                                        });
                                    $scope.loadingForwarderActivity = false;
                                }, (error) => { $scope.loadingForwarderActivity = false; });

                        }, 5000);
                    }
                },
                demurrage: {
                    data: {},
                    get: function (data) {
                        // TODO: fetch only once script is done loading
                        // send user token which the server would use to process dashboard data
                        let max = 0;
                        $scope.daysOfDemurrageActivity = data || $scope.daysOfDemurrageActivity;
                        $scope.loadingDemurrage = true;

                        // clear the existing chart
                        if (demurrageChart) {
                            demurrageChart.destroy();
                        }

                        // wait a few seconds
                        $timeout(() => {

                            $http({
                                method: 'POST',
                                url: api + '/importexport/dashboarddemurrageanalytics',
                                data: { Token: $rootScope.User.Login.Token, Days: $scope.daysOfDemurrageActivity },
                                headers: { 'Content-Type': 'application/json; charset=utf-8' }
                            })
                                .then(function (response) {
                                    // no data to draw Chart
                                    if (response.data === null || response.data === undefined
                                        || response.data.labels.length === 0) return;

                                    let rgbObj = appFactory.getRgbArray(response.data.datasets.length);
                                    $scope.dashboard.demurrage.data = response.data;

                                    // filter the date part of the data labels
                                    for (let i in $scope.dashboard.demurrage.data.labels) {
                                        $scope.dashboard.demurrage.data.labels[i] = $filter('date')($scope.dashboard.demurrage.data.labels[i], 'MMM dd');
                                    }
                                    for (let i in $scope.dashboard.demurrage.data.datasets) {
                                        $scope.dashboard.demurrage.data.datasets[i].backgroundColor = rgbObj.rgbaOpaque[i]; //'transparent';
                                        $scope.dashboard.demurrage.data.datasets[i].borderColor = rgbObj.rgb[i];
                                        $scope.dashboard.demurrage.data.datasets[i].lineTension = 0;
                                        $scope.dashboard.demurrage.data.datasets[i].pointRadius = $scope.dashboard.demurrage.data.datasets[i].pointHoverRadius = 10;
                                        //$scope.dashboard.demurrage.data.datasets[i].spanGaps = false;

                                        // get the maximum days gathered
                                        let o = $scope.dashboard.demurrage.data.datasets[i].data;
                                        for (let j in o) {
                                            if (o[j] !== null && o[j] > max) {
                                                max = o[j];
                                                break;
                                            }
                                        }
                                    }

                                    demurrageCanvas = chartjsFactory.setCanvas('canvas2');
                                    demurrageChart = chartjsFactory.createChart('line',
                                        $scope.dashboard.demurrage.data,
                                        {
                                            tooltips: {
                                                mode: 'label'
                                            },
                                            scales: {
                                                xAxes: [{
                                                    scaleLabel: {
                                                        display: true,
                                                        labelString: 'Cargo Dispatched (Date)'
                                                    }
                                                }],
                                                yAxes: [{
                                                    scaleLabel: {
                                                        display: true,
                                                        labelString: 'Demurrage (Days)'
                                                    },
                                                    ticks: {
                                                        beginAtZero: true,
                                                        stepSize: max > suggestedMax ? Math.ceil(max / suggestedMax) : 1,
                                                        suggestedMax: suggestedMax
                                                    }
                                                }]
                                            },
                                            legend: {
                                                display: false
                                                //position: 'right',
                                                //labels: {
                                                //    fontColor: 'rgb(255, 99, 132)'
                                                //}
                                            }
                                        });
                                    $scope.loadingDemurrage = false;
                                }, (error) => { $scope.loadingDemurrage = false; });
                        }, 5000);
                    }
                },
                maps: {
                    data: {}
                }
            };

            //Chart.defaults.global.elements.line.backgroundColor = 'transparent';

            // load dashboard import data
            $scope.dashboard.import.get();
            // load dashboard export data
            $scope.dashboard.export.get();
            // load dashboard shipment analysis


            // Analytics: loading variables
            $scope.loadingShipments = true;
            $scope.loadingDemurrage = true;
            $scope.loadingForwarderActivity = true;
            $scope.loadingTopImport = true;
            $scope.loadingTopExport = true;

            $scope.showActiveForwarder = false;
            $scope.showInactiveForwarder = false;

            $scope.toggleActiveForwarder = function () {
                if ($scope.showActiveForwarder) {
                    $scope.showActiveForwarder = false;
                } else {
                    $scope.showActiveForwarder = true;
                }
            };

            $scope.toggleInactiveForwarder = function () {
                if ($scope.showInactiveForwarder) {
                    $scope.showInactiveForwarder = false;
                } else {
                    $scope.showInactiveForwarder = true;
                }
            };


            // Analytics: Shipments - wait for DOM
            $timeout(() => {
                $scope.dashboard.shipments.get();
            });


            // Analytics: Demurrage - wait for DOM
            $timeout(() => {
                $scope.dashboard.demurrage.get();
            });

            // Analytics: Forwarders Activity - wait for DOM
            $timeout(() => {
                $scope.dashboard.forwarderActivity.get();
            });


            // Analytics: Top Import Countries
            $timeout(() => {
                appAnalytics.topImportCountries()
                    .then(function (data) {
                        // data returned
                        $scope.topImportCountries = data;
                        $scope.loadingTopImport = false;
                    },
                    (error) => {
                        // error occurred
                        $scope.loadingTopImport = false;
                    });
            }, 4000);



            // Analytics: Top Export Countries
            $timeout(() => {
                appAnalytics.topExportCountries()
                    .then(function (data) {
                        // data returned
                        $scope.topExportCountries = data;
                        $scope.loadingTopExport = false;
                    },
                    (error) => {
                        // error occurred
                        $scope.loadingTopExport = false;
                    });
            }, 4000);



            // TODO: Google Maps get Latitude and Longitude Information
            $http({
                method: 'GET',
                url: 'https://maps.googleapis.com/maps/api/geocode/json?address=ethiopia&key=' + $rootScope.googleApiKey,
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            })
                .then(function (response) {
                    $scope.dashboard.maps.data = response.data;
                });

        }]);
})();
(function () {
    'use strict';

    app.controller('commentCtrl', ['$scope', '$rootScope', '$state', 'appFactory', 'signalRHubProxy', 'commentService',
        function ($scope, $rootScope, $state, appFactory, signalRHubProxy, commentService) {

            let maxLength = 1000;    // maximum characters allowed as comment
            $scope.comments = [];
            $scope.comment = { CommentText: '' };


            // returns the total number of characters remaining
            $scope.getXtersLeft = function () {
                $scope.xtersLeft = appFactory.xtersLeft(maxLength, $scope.comment.CommentText.length);
            };


            // clear comment text and reset maximum xters left
            $scope.clearComment = function () {
                $scope.comment.CommentText = '';
                $scope.getXtersLeft();
            };


            $scope.clearComment();  // init xtersLeft and comment text

            $scope.closeWindow = function () {
                $scope.clearComment();
                $rootScope.showComment = false;
                appFactory.setModalOpen(false);
            };


            // listen for open comment
            $scope.$on('commentOpen', function (event, id) {
                $scope.comment.ImportExportID = parseInt(id);

                // get importExport comments thru service
                commentService.getComments(id)
                    .then(function (data) {
                        $scope.comments = data;
                    },
                    function (err) {
                        // handle error
                    });
            });


            $scope.submit = function () {
                // post comment and clear the comment box
                commentService.submitComment($scope.comment)
                    .then(function (data) {
                        $scope.clearComment();
                    }, function (err) {
                        appFactory.showDialog('Oops! Something went wrong.', true);
                    });
            };



            // SIGNALR CODES
            let commentProxy = signalRHubProxy(serverUrl, 'commentHub');
            commentProxy.on('commentAdded',
                function (comment) {
                    // add comment to matching importExport
                    if (comment.ImportExportID === $scope.comment.ImportExportID) {
                        $scope.comments.push(comment);
                    }
                });

        }]);

})();
(function () {
    'use strict';

    app.controller('supportCtrl', ['$scope', '$rootScope', '$state', 'appFactory', 'signalRHubProxy',
        function ($scope, $rootScope, $state, appFactory, signalRHubProxy) {

            $scope.message = "Welcome to Support!";

            // TODO: get support tickets

            // TODO: add new support

            // TODO: close a support ticket

        }]);

})();
(function () {
    app.controller('uploadCtrl', ['$http', '$scope', '$rootScope', '$state', '$sessionStorage', 'appFactory', 'Upload', '$timeout', function ($http, $scope, $rootScope, $state, $sessionStorage, appFactory, Upload, $timeout) {
        /**
        * <parameter>
        *      photoType: number
        * </parameter>
        * The photoType parameter accepts the following numbers and represent the following
        * 0 = USER_PROFILE_IMAGE
        * 1 = COMPANY_LOGO
        */

        // TODO: Create blob from existing base64 image
        // console.log('croppedDataUrl', $scope.croppedDataUrl);

        // disable <body> scrolling
        appFactory.setModalOpen(true);

        // close the upload window
        // enable <body> scrolling
        // return to parent state
        $scope.closeWindow = function () {
            appFactory.setModalOpen(false);
            if ($rootScope.photoType === 0) $state.go('account');
            else $state.go('account.company');
        }

        let url_attachment;
        let url_delete;
        switch ($rootScope.photoType) {
            case 0:
                url_attachment = '/account/postprofilephoto';
                url_delete = '/account/deleteprofilephoto';
                break;
            default:
                url_attachment = '/account/postcompanyphoto';
                url_delete = '/account/deletecompanyphoto';
        }

        // returns boolean - check if photo exists
        $scope.noPhoto = function () {
            switch ($rootScope.photoType) {
                case 0:
                    if ($rootScope.User.Person.Photo === '') return true;
                    return false;
                    break;
                default:
                    if ($rootScope.User.Company.Photo === '') return true;
                    return false;
            }
        }

        // delete profile photo
        $scope.delete = function () {

            $http({
                method: 'DELETE',
                url: api + url_delete,
                data: $rootScope.User,
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            })
                .then((response) => {
                    // clear the profile url
                    if ($rootScope.photoType === 0) {
                        $rootScope.User.Person = response.data;
                    } else {
                        $rootScope.User.Company = response.data;
                    }

                    // update user storage and view
                    $sessionStorage.__user = $rootScope.User;
                    appFactory.setDataImage();

                    $scope.closeWindow();
                    $timeout(() => {
                        $state.reload();
                    }, 200);
                    appFactory.showDialog('Profile photo deleted.');
                },
                (error) => {
                    $scope.closeWindow();
                    appFactory.showDialog('Unable to delete profile at this time.', true);
                });
        };
        
        // upload the profile picture
        $scope.upload = function (dataUrl, name) {
            let data = { file: Upload.dataUrltoBlob(dataUrl, name) };

            if ($rootScope.photoType === 0) {
                data.personId = $rootScope.User.Person.ID;
            } else {
                data.companyId = $rootScope.User.Company.ID;
            }

            Upload.upload({
                url: api + url_attachment,
                data: data
            }).then(function (response) {
                $timeout(function () {
                    if ($rootScope.photoType === 0) {
                        // set the user profile base64 img
                        $rootScope.User.Person.PhotoDataUrl = dataUrl;
                    } else {
                        // set the company profile base64 img
                        $rootScope.User.Company.PhotoDataUrl = dataUrl;
                    }

                    $scope.closeWindow();
                    appFactory.showDialog('Profile photo updated.');
                });
            }, function (response) {
                if (response.status > 0) $scope.errorMsg = response.status
                    + ': ' + response.data;
            }, function (evt) {
                $scope.progress = parseInt(100.0 * evt.loaded / evt.total);
            });
        };
        
    }]);
})();
(function () {
    'use strict';
    
    // TODO: Remove Custom Reports on go live until it's remodeled in the future
    /*
     * CUSTOM REPORTS FROM OLD MARILOG PROJECT
     * This should be remodeled in the future as it is not guaranteed against SQL Injections
     * plus the records returned are not accurate due to impossibility of
     * accomodating all JOIN scenarios in a single SQL Statement.
    */
    app.controller('reportCustomCtrl', ['$scope', '$rootScope', '$http', '$state', '$sessionStorage', 'appFactory',
        function ($scope, $rootScope, $http, $state, $sessionStorage, appFactory) {
            if (!$rootScope.User || $rootScope.User === null | undefined) { $state.go("login"); }

            // $scope.formData { ReportName:'', CompanyID: '', Queries: [] }
            // this is an angular model that holds information about 
            // the report name, the kind of user based on CompanyID and sql queries which are sent to the Web API
            $scope.formData = {
                'ReportName': '',
                'CompanyID': $rootScope.User.Company.ID,
                'Queries': [],
                'ConfigStatsTimes': $sessionStorage.configStatsTimes
            };
            $scope.message = '';



            // ------------------------------- START OF DIRECTIVE -------

            //------------------------------------- M A P P I N G   O F   C O L U M N S ---------------
            // CUSTOM REPORTS SECTION
            $scope.dbTables = null;
            $scope.tableColumns = [];
            $scope.column = {};

            // hide or show the mapping settings page in customs report 
            $scope.mapIsHidden = true;
            $scope.controlHidden = true;

            $scope.isFirstClause = true;
            $scope.reportQuery = null;
            $scope.customReport = [];
            $scope.customReportHeaders = [];
            $scope.currentColumnMapping = {};
            $scope.initCustomReportQuery = function () {
                $scope.customReportQuery = {
                    'Queries': [],
                    'CompanyTypeID': $rootScope.User.Company.CompanyTypeID,
                    'CompanyID': $rootScope.User.Company.ID
                };
            };

            // gets all possible column headers for custom reports
            $scope.getCustomReportHeaders = function () {
                $http({
                    method: 'GET',
                    url: api + '/reports' + '/ColumnHeaders',
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $scope.customReportHeaders = response.data;
                    });
            };

            $scope.initCustomReportQuery();
            $scope.getCustomReportHeaders();

            $scope.refillColumns = function () {
                // set ColumnHeaders
                if ($scope.customReportHeaders.length === 0) {
                    $scope.getCustomReportHeaders();
                }

                if ($scope.currentColumnMapping === null
                    || $scope.currentColumnMapping === undefined
                    || $scope.currentColumnMapping.MappingModelParams === undefined) return;

                // refill the colums for each model params table
                $scope.tableColumns = [];
                for (var i = 0; i < $scope.currentColumnMapping.MappingModelParams.length; i++) {
                    $scope.getTableColumns($scope.currentColumnMapping.MappingModelParams[i].TableName, i);
                }
            };

            $scope.getTableColumns = function (t, index) {
                $http({
                    method: 'GET', url: api + '/reports' + '/Columns?tableName=' + t, headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $scope.tableColumns[index] = response.data;
                    });
            };

            $scope.getDBTables = function (currentMap) {
                // clear current report
                $scope.customReport = [];

                if (currentMap === null || currentMap === undefined || currentMap.Name === undefined) {
                    $scope.currentColumnMapping = {
                        'ColumnMappings': [],
                        'MappingModelParams': []
                    };

                    // initialize the first 5 column mappings as true
                    // set the rest as false
                    for (var i in $scope.customReportHeaders) {
                        if (i < 5) {
                            $scope.currentColumnMapping.ColumnMappings.push(true);
                            continue;
                        }
                        $scope.currentColumnMapping.ColumnMappings.push(false);
                    }

                }
                else {
                    $scope.currentColumnMapping = currentMap;
                }

                if ($scope.dbTables === null) {
                    $http({
                        method: 'GET', url: api + '/reports' + '/Tables', headers: { 'Content-Type': 'application/json' }
                    })
                        .then(function (response) {
                            $scope.dbTables = response.data;

                            // Then load Columns of the tables in $scope.currentColumnMapping.MappingModelParams[]
                            $scope.refillColumns();
                        });
                }
                else {
                    $scope.refillColumns();
                }
            };


            // push a new object to currentColumnMapping.MappingModelParams[]
            // and load dbTables
            $scope.addNewQuery = function () {
                if ($scope.currentColumnMapping === null
                    || $scope.currentColumnMapping === undefined
                    || $scope.currentColumnMapping.MappingModelParams === undefined) {
                    $scope.getDBTables($scope.currentColumnMapping);
                }

                $scope.currentColumnMapping.MappingModelParams.push({});
            };


            $scope.rptQueryBuilder = function (lOp, tbl, col, op, v) {
                if ($scope.reportQuery !== null) $scope.isFirstClause = false;
                if (lOp === undefined) lOp = "";
                if (v === undefined) v = "";

                if (op === "LIKE") {
                    $scope.reportQuery = " " + lOp + " " + tbl + "." + col + " " + op + " '%" + v + "%'";
                }
                else {
                    $scope.reportQuery = " " + lOp + " " + tbl + "." + col + " " + op + " '" + v + "'";
                }

                return $scope.reportQuery;
            };


            // Reset all models used to build query including currentMappingParams
            $scope.resetQuery = function () {
                $scope.isFirstClause = true;
                $scope.reportQuery = null;
                $scope.mappingStatusMessage = undefined;
                $scope.initCustomReportQuery();
            };

            $scope.resetAllQuery = function () {
                $scope.currentColumnMapping = null;
                $scope.resetQuery();
            };

            $scope.submitQuery = function () {
                $scope.mappingStatusMessage = undefined;
                // build all conditions from $scope.currentColumnMapping.MappingModelParams
                var o = $scope.currentColumnMapping.MappingModelParams;
                for (var i = 0; i < o.length; i++) {
                    $scope.customReportQuery.Queries.push($scope.rptQueryBuilder(o[i].Lop, o[i].TableName, o[i].ColumnName, o[i].Op, o[i].Val));
                }

                // send queries to the server
                $http({
                    method: 'POST',
                    url: api + '/reports' + '/Custom',
                    data: $scope.customReportQuery,
                    cache: false,
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        $scope.customReport = response.data;
                    }, function () {
                        $scope.customReport = [];
                    });

                // reset query
                $scope.resetQuery();
            };

            $scope.hideMapping = function () {
                appFactory.setModalOpen(false);
                $scope.mapIsHidden = true;
            };

            $scope.showMappingView = function () {
                appFactory.setModalOpen(true); // remove extra scrollbar on <body>
                $scope.controlHidden = false;
                $scope.mapIsHidden = !$scope.mapIsHidden;

                $scope.getDBTables($scope.currentColumnMapping);
            };

        }]);

})();
(function () {
    'use strict';

    app.controller('activityController', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');        

            function confirmActivityDelete(d, pindex, cindex, model, message, fnc) {
                $scope.confirmed = false;
                if (!d && model) {
                    d = model.d;
                    $scope.confirmed = true;
                    $rootScope.closeDialog(); 
                }               

                // show confirm dialog before recycle
                if (!$scope.confirmed) {
                    appFactory.showDialog(message.replace('#ACTIVITY#', d.ID), null, true, fnc);
                    model = {
                        d: d,
                        pindex: pindex,
                        cindex: cindex
                    };
                }

                return model;
            }

            $scope.deleteActivity = function (d, pindex, cindex) {
                $scope.recycleData = confirmActivityDelete(d,
                    pindex,
                    cindex,
                    $scope.recycleData,
                    'You will no longer have access to this activity (#BREAKBULK#). Do you want to continue',
                    $scope.deleteActivity);

                if (!$scope.confirmed) return false;

                $http({
                    method: 'DELETE',
                    url: api + '/maritime/DeleteDailyBreakBulk/' + $scope.recycleData.d.ID,
                    data: { ID: $scope.recycleData.d.ID, Token: $rootScope.User.Login.Token },
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        if ($scope.breakBulkActivities === undefined)
                            $scope.groupedBreakBulks[$scope.recycleData.pindex].value[$scope.recycleData.cindex].Terminated = true;

                        appFactory.showDialog('Activity has been recycled.');
                        $rootScope.refresh();
                    },
                        function (error) {
                            appFactory.showDialog('Unable to delete activity.', true);
                        });
            };

            $scope.deleteMultiModal = function (d, pindex, cindex) {
                $scope.recycleData = confirmActivityDelete(d,
                    pindex,
                    cindex,
                    $scope.recycleData,
                    'You will no longer have access to this activity (#MULTIMODAL#). Do you want to continue',
                    $scope.deleteMultiModal);

                if (!$scope.confirmed) return false;

                $http({
                    method: 'DELETE',
                    url: api + '/maritime/DeleteDailyMultiModalTransport/' + $scope.recycleData.d.ID,
                    data: { ID: $scope.recycleData.d.ID, Token: $rootScope.User.Login.Token },
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        if ($scope.multiModalActivities === undefined)
                            $scope.groupedMultiModal[$scope.recycleData.pindex].value[$scope.recycleData.cindex].Terminated = true;

                        appFactory.showDialog('Activity has been recycled.');
                        $rootScope.refresh();
                    },
                        function (error) {
                            appFactory.showDialog('Unable to delete activity.', true);
                        });
            };

            $scope.deleteUniModalActivity = function (d, pindex, cindex) {
                $scope.recycleData = confirmActivityDelete(d,
                    pindex,
                    cindex,
                    $scope.recycleData,
                    'You will no longer have access to this activity (#UNIMODAL#). Do you want to continue',
                    $scope.deleteUniModalActivity);

                if (!$scope.confirmed) return false;

                $http({
                    method: 'DELETE',
                    url: api + '/maritime/DeleteDailyUniModalTransport/' + $scope.recycleData.d.ID,
                    data: { ID: $scope.recycleData.d.ID, Token: $rootScope.User.Login.Token },
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        if ($scope.uniModalActivities === undefined)
                            $scope.groupedUniModal[$scope.recycleData.pindex].value[$scope.recycleData.cindex].Terminated = true;

                        appFactory.showDialog('Activity has been recycled.');
                        $rootScope.refresh();
                    },
                        function (error) {
                            appFactory.showDialog('Unable to delete activity.', true);
                        });
            };

            $scope.preview = {
                'breakBulk': {
                    openWindow: false,
                    data: {},
                    show: function (o) {
                        $scope.preview.breakBulk.data = o;
                        $scope.preview.breakBulk.openWindow = true;
                        appFactory.setModalOpen(true);
                    },
                    closeWindow: function () {
                        $scope.preview.breakBulk.data = {};
                        $scope.preview.breakBulk.openWindow = false;
                        appFactory.setModalOpen(false);
                    }
                },
                'multiModal': {
                    openWindow: false,
                    data: {},
                    show: function (o) {
                        $scope.preview.multiModal.data = o;
                        $scope.preview.multiModal.openWindow = true;
                        appFactory.setModalOpen(true);
                    },
                    closeWindow: function () {
                        $scope.preview.multiModal.data = {};
                        $scope.preview.multiModal.openWindow = false;
                        appFactory.setModalOpen(false);
                    }
                },
                'uniModal': {
                    openWindow: false,
                    data: {},
                    show: function (o) {
                        //  console.log('yay');
                        $scope.preview.uniModal.data = o;
                        $scope.preview.uniModal.openWindow = true;
                        appFactory.setModalOpen(true);
                    },
                    closeWindow: function () {
                        $scope.preview.uniModal.data = {};
                        $scope.preview.uniModal.openWindow = false;
                        appFactory.setModalOpen(false);
                    }
                }
            };
        }]);

    app.controller('indexBreakBulkCtrl', ['$scope', '$rootScope', '$http', '$sessionStorage', '$state', 'appFactory', 'Upload', '$timeout', '$filter', 'datepickerProvider',
        function ($scope, $rootScope, $http, $sessionStorage, $state, appFactory, Upload, $timeout, $filter) {

            // console.log('this page is reached');
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');
           
            /////////////////////////////////////////////
            $scope.searchBreakBulk = function (searchIsNew) {
                if ($scope.searchText === undefined) $scope.searchText = '';

                if (searchIsNew) {
                    $scope.breakBulkActivities = [];
                    $scope.groupedBreakBulks = [];
                }

                appFactory.getBreakBulkActivities($scope.breakBulkActivities.length, $scope.breakBulkActivities, $scope.searchText)
                    .then(function (data) {
                        if (data !== null) {
                            $scope.breakBulkActivities = data.value;
                            $scope.groupedBreakBulks = $filter('groupByDate')($scope.breakBulkActivities, 'DateInserted');
                            $scope.odataInfo = data.odataInfo;
                            appFactory.prepCards();

                        }
                    });
            };

            // this determines what status is available for selection
            $scope.impExpTypeId = 1;

            // this method is called when add break bulk is clicked
            // it gets few of the value data required in the forms
            $scope.initUISelections = function () {
                appFactory.getCountries();
            };

            // loads all required values and clean up
            $scope.initUISelections();

            // init variables for add/edit break bulk
            $scope.initBreakBulk = function () {

                // Create a new break bulk object
                $scope.newBreakBulk = {
                    companyID: $rootScope.User.Person.CompanyID,
                    impExpTypeId: 1,
                    createdBy: $rootScope.User.Person.ID,
                };
            };

            // save the new break bulk object
            $scope.createBreakBulk = function () {
                // disable the send button
                //$scope.disableButton = true;

                $http({
                    method: 'POST',
                    url: api + '/maritime/PostDailyBreakBulk',
                    data: $scope.newBreakBulk,
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        // go to the break bulk list page and refresh the list
                        appFactory.showDialog('Break bulk submitted successfully.');
                        //$rootScope.refresh();                     
                        //$state.go('breakbulk');                     

                    },
                        function (error) {
                            $scope.disableButton = false; // enable the send button
                            appFactory.showDialog('Break bulk was not submitted.', true);
                          
                        });
            };

            //
            // init variables for update break bulk
            $scope.editActivity = function (d) {                      
                // init break bulk object
                $scope.editBreakBulk = {
                    id: d.ID,
                    companyId: d.CompanyID,
                    impExpTypeId: d.ImpExpTypeID,   
                    portOfLoadingID: d.PortOfLoading,
                    daysAtPort: d.DaysAtPort,
                    noOfVehicle: d.NoOfVehicle,
                    storedMetalMetricTon: d.StoredMetalMetricTon,
                    transportedToCountryMetricTon: d.TransportedToCountryMetricTon,
                    dateInitiated: d.DateInitiated,                   
                    changedBy: $rootScope.User.Person.ID,                    
                    remark: d.Remark,                  
                };
                $state.go('breakbulk.edit')
            };

            // save the updated break bulk object
            $scope.updateBreakBulk = function () {

                $http({
                    method: 'PUT',
                    url: api + '/maritime/PutDailyBreakBulk',
                    data: $scope.editBreakBulk,
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        // go to the break bulk list page and refresh the list
                        // disable the save button
                        $scope.disableButton = true;
                        appFactory.showDialog('Break bulk updated successfully.');
                      //  console.log('responseUpdate', response);

                    },
                        function (error) {
                            $scope.disableButton = false;
                            appFactory.showDialog('Break bulk was not updated.', true);
                          //  console.log('responseUpdateErr', error);
                        });
            };   

            // init breakbulk then load existing breakbulk collection
            $scope.breakBulkActivities = [];
            $scope.getBreakBulkActivities = function () {
                // get ongoing breakbulk activities for company
                appFactory.getBreakBulkActivities($scope.breakBulkActivities.length, $scope.breakBulkActivities)
                    .then(function (data) {
                        if (data !== null) {
                            $scope.breakBulkActivities = data.value;
                            $scope.groupedBreakBulks = $filter('groupByDate')($scope.breakBulkActivities, 'DateInserted');
                            $scope.odataInfo = data.odataInfo;
                            appFactory.prepCards();
                        }
                    });
            };

            $scope.getBreakBulkActivities();

        }]);   

    ///
    app.controller('indexMultiModalCtrl', ['$scope', '$rootScope', '$http', '$sessionStorage', '$state', 'appFactory', 'Upload', '$timeout', '$filter',
        function ($scope, $rootScope, $http, $sessionStorage, $state, appFactory, Upload, $timeout, $filter) {

            // console.log('this page is reached');
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');

            $scope.searchMultiModal = function (searchIsNew) {
                if ($scope.searchText === undefined) $scope.searchText = '';

                if (searchIsNew) {
                    $scope.multiModalActivities = [];
                    $scope.groupedMultiModal = [];
                }

                appFactory.getMultiModalActivities($scope.multiModalActivities.length, $scope.multiModalActivities, $scope.searchText)
                    .then(function (data) {
                        if (data !== null) {
                            $scope.multiModalActivities = data.value;
                            $scope.groupedMultiModal = $filter('groupByDate')($scope.multiModalActivities, 'DateInserted');
                            $scope.odataInfo = data.odataInfo;
                            appFactory.prepCards();

                        }
                    });
            };

            // this determines what status is available for selection
            $scope.impExpTypeId = 1;

            // this method is called when add multi modal is clicked
            // it gets few of the value data required in the forms
            $scope.initUISelections = function () {
                appFactory.getCountries();
            };

            // loads all required values and clean up
            $scope.initUISelections();

            // init variables for add/edit break bulk
            $scope.initMultiModal = function () {

                // Create a new break bulk object
                $scope.newMultiModal = {
                    companyID: $rootScope.User.Person.CompanyID,
                    impExpTypeId: 1,
                    createdBy: $rootScope.User.Person.ID,
                };
            };

            // save the new multi modal object
            $scope.createMultiModal = function () {
                // disable the send button
                //$scope.disableButton = true;

                $http({
                    method: 'POST',
                    url: api + '/maritime/PostDailyMultiModalTransport',
                    data: $scope.newMultiModal,
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        // go to the multi modal list page and refresh the list
                        appFactory.showDialog('Multi Modal submitted successfully.');
                        // $rootScope.refresh();                     
                        // $state.go('multimodal');                     

                    },
                        function (error) {
                            $scope.disableButton = false; // enable the send button
                            appFactory.showDialog('Multi Modal was not submitted.', true);

                        });
            };
                       
            // init variables for update multi modal
            $scope.editMultiModalActivity = function (d) {
                // init break bulk object
                $scope.editMultiModal = {
                    id: d.ID,
                    companyID: d.CompanyID,
                    impExpTypeId: d.ImpExpTypeID,                    
                    changedBy: $rootScope.User.Person.ID,
                    container20Ft: d.Container20Ft,
                    container40Ft: d.Container40Ft,
                    tEU: d.TEU,
                    roRo: d.RoRo,
                    vehicleTransport: d.VehicleTransport,
                    trainTransport: d.TrainTransport,
                    containerAtPort20Ft: d.ContainerAtPort20Ft,
                    containerAtPort40Ft: d.ContainerAtPort40Ft,
                    tEUAtPort: d.TEUAtPort,
                    averageContainerStay: d.AverageContainerStay,
                    dateInitiated: d.DateInitiated,
                    remark: d.Remark,
                };
                $state.go('multimodal.edit')
            };

            // save the updated multi mo object
            $scope.updateMultiModal = function () {

                $http({
                    method: 'PUT',
                    url: api + '/maritime/PutDailyMultiModalTransport',
                    data: $scope.editMultiModal,
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        // go to the multi modal list page and refresh the list
                        // disable the save button
                        $scope.disableButton = true;
                        appFactory.showDialog('Multi Modal updated successfully.');
                       console.log('responseUpdate', response);

                    },
                        function (error) {
                            $scope.disableButton = false;
                            appFactory.showDialog('Multi Modal was not updated.', true);
                           console.log('responseUpdateErr', error);
                        });
            };

            // init multimodal then load existing breakbulk collection
            $scope.multiModalActivities = [];
            $scope.getMultiModalActivities = function () {
                // get ongoing multimodal activities for company
                appFactory.getMultiModalActivities($scope.multiModalActivities.length, $scope.multiModalActivities)
                    .then(function (data) {
                        if (data !== null) {
                            $scope.multiModalActivities = data.value;
                            $scope.groupedMultiModal = $filter('groupByDate')($scope.multiModalActivities, 'DateInserted');
                            $scope.odataInfo = data.odataInfo;
                            appFactory.prepCards();
                        }
                    });
            };

            $scope.getMultiModalActivities();


        }]);

    ///
    app.controller('indexUniModalCtrl', ['$scope', '$rootScope', '$http', '$sessionStorage', '$state', 'appFactory', 'Upload', '$timeout', '$filter',
        function ($scope, $rootScope, $http, $sessionStorage, $state, appFactory, Upload, $timeout, $filter) {
            // console.log('this page is reached');
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');
            $scope.searchUniModal = function (searchIsNew) {
                if ($scope.searchText === undefined) $scope.searchText = '';

                if (searchIsNew) {
                    $scope.uniModalActivities = [];
                    $scope.groupedUniModal = [];
                }

                appFactory.getUniModalActivities($scope.uniModalActivities.length, $scope.uniModalActivities, $scope.searchText)
                    .then(function (data) {
                        if (data !== null) {
                            $scope.uniModalActivities = data.value;
                            $scope.groupedUniModal = $filter('groupByDate')($scope.uniModalActivities, 'DateInserted');
                            $scope.odataInfo = data.odataInfo;
                            appFactory.prepCards();

                        }
                    });
            };

            // this determines what status is available for selection
            $scope.impExpTypeId = 1;

            // this method is called when add unimodal is clicked
            // it gets few of the value data required in the forms
            $scope.initUISelections = function () {
                appFactory.getCountries();
            };

            // loads all required values and clean up
            $scope.initUISelections();

            // init variables for add/edit unimodal
            $scope.initUniModal = function () {

                // Create a new unimodal object
                $scope.newUniModal = {
                    companyID: $rootScope.User.Person.CompanyID,
                    impExpTypeId: 1,
                    createdBy: $rootScope.User.Person.ID,
                };
            };

            // save the new unimodal object
            $scope.createUniModal = function () {
                // disable the send button
                //$scope.disableButton = true;

                $http({
                    method: 'POST',
                    url: api + '/maritime/PostDailyUniModalTransport',
                    data: $scope.newUniModal,
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        // go to the unimodal list page and refresh the list
                        appFactory.showDialog('Multi-modal submitted successfully.');                                          
                        //$state.go('unimodal');

                    },
                        function (error) {
                            $scope.disableButton = false; // enable the send button
                            appFactory.showDialog('Multi-modal was not submitted.', true);

                        });
            };

            //
            // init variables for update unimodal
            $scope.editUniModalActivity = function (d) {
                // init unimodal object
                $scope.editUniModal = {
                    id: d.ID,
                    companyId: $rootScope.User.Person.CompanyID,
                    changedBy: $rootScope.User.Person.ID,
                    impExpTypeId: 1,
                    box: d.Box,
                    tEU: d.TEU,
                    roRo: d.RoRo,
                    vehicleTransport: d.VehicleTransport,
                    trainTransport: d.TrainTransport,
                    dctContainerAtPort20Ft: d.DctContainerAtPort20Ft,
                    dctContainerAtPort40Ft: d.DctContainerAtPort40Ft,
                    dmpContainerAtPort20Ft: d.DmpContainerAtPort20Ft,
                    dmpContainerAtPort40Ft: d.DmpContainerAtPort40Ft,
                    tEUAtPort: d.TEUAtPort,
                    dctAverageContainerStay: d.DctAverageContainerStay,
                    dmpAverageContainerStay: d.DmpAverageContainerStay,
                    dateInitiated: d.DateInitiated,
                    remark: d.Remark,
                };
                $state.go('unimodal.edit')
            };

            // save the updated unimodal object
            $scope.updateUniModal = function () {

                $http({
                    method: 'PUT',
                    url: api + '/maritime/PutDailyUniModalTransport',
                    data: $scope.editUniModal,
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        // go to the unimodal list page and refresh the list
                        // disable the save button
                        $scope.disableButton = true;
                        appFactory.showDialog('Uni-modal updated successfully.');
                        //  console.log('responseUpdate', response);

                    },
                        function (error) {
                            $scope.disableButton = false;
                            appFactory.showDialog('Uni-modal was not updated.', true);
                            //  console.log('responseUpdateErr', error);
                        });
            };

            // init unimodal then load existing breakbulk collection
            $scope.uniModalActivities = [];
            $scope.getUniModalActivities = function () {
                // get  unimodal activities
                appFactory.getUniModalActivities($scope.uniModalActivities.length, $scope.uniModalActivities)
                    .then(function (data) {
                        if (data !== null) {
                            $scope.uniModalActivities = data.value;
                            $scope.groupedUniModal = $filter('groupByDate')($scope.uniModalActivities, 'DateInserted');
                            $scope.odataInfo = data.odataInfo;
                            appFactory.prepCards();
                        }
                    });
            };

            $scope.getUniModalActivities();

        }]);  
 
    ///
    app.controller('indexfreeZoneCtrl', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);

    app.controller('createFreeZoneCtrl', ['$scope', '$rootScope', '$http', '$state',
        function ($scope, $rootScope, $http, $state) {
            // pass user token to Web API
            // go to home, if the user is not logged in or does not have edit user privilege
            // just incase the user paste in the url for manage users
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');

            $http({
                method: 'POST',
                url: api + '/maritime/PostDailyBreakBulk',
                data: $rootScope.User.Login,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    $scope.perms = response.data;
                    if ($scope.perms.EditUser === false)
                        $state.go('home');
                });
        }]);

    app.controller('editFreeZoneCtrl', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);

    ///
    app.controller('indexnonePackedCtrl', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);

    app.controller('createNonePackedCtrl', ['$scope', '$rootScope', '$http', '$state',
        function ($scope, $rootScope, $http, $state) {
            // pass user token to Web API
            // go to home, if the user is not logged in or does not have edit user privilege
            // just incase the user paste in the url for manage users
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');

            $http({
                method: 'POST',
                url: api + '/maritime/PostDailyBreakBulk',
                data: $rootScope.User.Login,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    $scope.perms = response.data;
                    if ($scope.perms.EditUser === false)
                        $state.go('home');
                });
        }]);

    app.controller('editNonePackedCtrl', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);

    ///
    app.controller('indexoilTransportCtrl', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);

    app.controller('createOilTransportCtrl', ['$scope', '$rootScope', '$http', '$state',
        function ($scope, $rootScope, $http, $state) {
            // pass user token to Web API
            // go to home, if the user is not logged in or does not have edit user privilege
            // just incase the user paste in the url for manage users
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');

            $http({
                method: 'POST',
                url: api + '/maritime/PostDailyBreakBulk',
                data: $rootScope.User.Login,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    $scope.perms = response.data;
                    if ($scope.perms.EditUser === false)
                        $state.go('home');
                });
        }]);

    app.controller('editOilTransportCtrl', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);

    ///
    app.controller('indexDjiboutiTajuraCtrl', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);

    app.controller('createDjiboutiTajuraCtrl', ['$scope', '$rootScope', '$http', '$state',
        function ($scope, $rootScope, $http, $state) {
            // pass user token to Web API
            // go to home, if the user is not logged in or does not have edit user privilege
            // just incase the user paste in the url for manage users
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');

            $http({
                method: 'POST',
                url: api + '/maritime/PostDailyBreakBulk',
                data: $rootScope.User.Login,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    $scope.perms = response.data;
                    if ($scope.perms.EditUser === false)
                        $state.go('home');
                });
        }]);

    app.controller('editDjiboutiTajuraCtrl', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);
})();
app.controller('datepickerPopupController', ['$scope', 'uibDateParser', 'datepickerProvider', function ($scope, uibDateParser, datepickerProvider) {
    // initialize the dateOptions
    $scope.dateOptions = datepickerProvider.getDateOptions();

    $scope.required = false;

    $scope.today = function () {
        $scope.dt = new Date();
    };

    // clears the date
    $scope.clear = function () {
        $scope.dt = null;
    };

    // dateOptions with a class illustration
    $scope.inlineOptions = {
        customClass: getDayClass,
        minDate: new Date(),
        showWeeks: true
    };


    $scope.toggleMin = function () {
        $scope.inlineOptions.minDate = !$scope.inlineOptions.minDate ? null : new Date();
        $scope.dateOptions.minDate = $scope.inlineOptions.minDate;
    };


    $scope.openDt = function () {
        $scope.popupDt.opened = true;
    };

    $scope.popupDt = {
        opened: false
    };


    $scope.setDate = function (year, month, day) {
        $scope.dt = new Date(year, month, day);
    };

    // takes string date to set $scope.dt and returns the value
    $scope.setDate2 = function (dt) {
        return $scope.dt = new Date(dt);
    };

    $scope.formats = datepickerProvider.getFormats();
    $scope.format = $scope.formats[0];
    $scope.altInputFormats = datepickerProvider.getFormats();

 

    var tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    var afterTomorrow = new Date();
    afterTomorrow.setDate(tomorrow.getDate() + 1);
    $scope.events = [
        {
            date: tomorrow,
            status: 'full'
        },
        {
            date: afterTomorrow,
            status: 'partially'
        }
    ];

    function getDayClass(data) {
        var date = data.date,
            mode = data.mode;
        if (mode === 'day') {
            var dayToCheck = new Date(date).setHours(0, 0, 0, 0);

            for (var i = 0; i < $scope.events.length; i++) {
                var currentDay = new Date($scope.events[i].date).setHours(0, 0, 0, 0);

                if (dayToCheck === currentDay) {
                    return $scope.events[i].status;
                }
            }
        }

        return '';
    }
}]);

app.controller('timepickerController', ['$scope', '$log', function ($scope, $log) {
    $scope.mytime = new Date();

    $scope.hstep = 1;
    $scope.mstep = 5;

    $scope.options = {
        hstep: [1, 2, 3],
        mstep: [1, 5, 10, 15, 25, 30]
    };

    $scope.ismeridian = true;
    $scope.toggleMode = function () {
        $scope.ismeridian = !$scope.ismeridian;
    };

    $scope.update = function () {
        var d = new Date();
        d.setHours(14);
        d.setMinutes(0);
        $scope.mytime = d;
    };

    $scope.changed = function () {
        $log.log('Time changed to: ' + $scope.mytime);
    };

    $scope.clear = function () {
        $scope.mytime = null;
    };
}]);
(function () {

    app.directive('viewSessionTimeout', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/sessiontimeout.html'
        };
    });
    app.directive('loader', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/loader.html'
        };
    });
    app.directive('reportNoResult', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/report_no_result.html'
        };
    });
    app.directive('dirComment', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/comment.html',
            controller: 'commentCtrl'
        };
    });
    app.directive('dirLoading', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/loading.html'
        };
    });

    app.directive('dirAnalyticsShipments', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/analytics_shipments.html'
        };
    });    
    app.directive('dirAnalyticsForwardersActivity', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/analytics_forwarder_activity.html'
        };
    });
    app.directive('dirAnalyticsDemurrage', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/analytics_demurrage.html'
        };
    });
    app.directive('dirAnalyticsTopImportCountries', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/analytics_top_import_countries.html'
        };
    });
    app.directive('dirAnalyticsTopExportCountries', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/analytics_top_export_countries.html'
        };
    });

    app.directive('dirMenuTop', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/menu_top.html'
        };
    });
    app.directive('dirAddItem', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/add_item_fullscreen.html'
        };
    });
    app.directive('dirPasswordChange', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/passwordchange.html'
        };
    });
    app.directive('dirNationalBankSearch', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/dashboard_bank_search_view.html',
            controller: 'nationalBankCtrl'
        };
    });
    app.directive('dirConsigneeView', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/dashboard_consignee_data_view.html',
            controller: 'consigneeCtrl'
        };
    });
    app.directive('dirTinOrLcData', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/tin_or_lc_data.html'
        };
    });  
    app.directive('dirCreateCompany', function () {
        return {
            restrict: 'E',
            transclude: true,
            controller: 'createCompanyCtrl',
            templateUrl: 'views/directives/create_company.html'
            //link: function(scope, element, attrs){
            //},
        };
    });

    app.directive('dirForwarderUpdateConsigneeView', function () {
        return {
            restrict: 'E',
            transclude: true,
            controller: 'importExportCtrl',
            templateUrl: 'views/directives/ff_update_consignee_view.html'
        };
    });
    app.directive('dirImportExportUpdateCostView', function () {
        return {
            restrict: 'E',
            transclude: true,
            controller: 'importExportCtrl',
            templateUrl: 'views/directives/importexport_update_cost_view.html'
        };
    });

    app.directive('dirExportSummary', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/export_summary.html'
        };
    });
    app.directive('dirImportSummary', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/import_summary.html'
        };
    });

    app.directive('dirImportView', function () {
        return {
            require: 'importController',
            restrict: 'E',
            transclude: true,
            controller: 'importExportCtrl',
            templateUrl: 'views/directives/imports_view.html'
        };
    });
    app.directive('dirImportToolbox', function () {
        return {
            restrict: 'EA',
            transclude: true,
            templateUrl: 'views/directives/import_toolbox.html'
        };
    });
    app.directive('dirImportBillPreview', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/import_bill_preview.html'
        };
    });

    app.directive('dirExportView', function () {
        return {
            require: 'exportController',
            restrict: 'E',
            transclude: true,
            controller: 'importExportCtrl',
            templateUrl: 'views/directives/exports_view.html'
            //link: function(scope, element, attrs){
            //},
        };
    });
    app.directive('dirExportToolbox', function () {
        return {
            restrict: 'EA',
            transclude: true,
            templateUrl: 'views/directives/export_toolbox.html'
        };
    });
    app.directive('dirExportBillPreview', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/export_bill_preview.html'
        };
    });

    app.directive('dirOptionsMobileWindow', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/options_more_window.html'
        };
    });

    app.directive('dirStatusUpdateView', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/status_update_view.html'
        };
    });
    app.directive('dirProblemUpdateView', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/problem_update_view.html'
        };
    });
    app.directive('dirUserProfileView', function () {
        return {
            controller: 'accountCtrl',
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/account_user_profile_view.html'
        };
    });
    app.directive('dirUserLinks', ['$http', '$rootScope', function ($http, $rootScope) {
        return {
            restrict: 'E',
            transclude: true,
            template: '<ng-include src="tmpl"/>',
            controller: 'accountCtrl',
            scope: '=',
            compile: function (tElem, tAttrs) {
                return {
                    pre: function preLink(scope, iElement, iAttrs, controller) {
                        scope.tmpl = 'views/directives/account_menu_view_user.html';
                    },
                    post: function postLink(scope, iElement, iAttrs, controller) {
                        // pass user token to Web API
                        // if the user has edit user privilege, load admin menu
                        $http({
                            method: 'POST',
                            url: api + '/account/getuserperm',
                            data: $rootScope.User.Login,
                            headers: { 'Content-Type': 'application/json' }
                        })
                            .then(function (response) {
                                // change the template if the user has EditUser privilege
                                scope.perms = response.data;
                                if (scope.perms.EditUser === true)
                                    scope.tmpl = 'views/directives/account_menu_view_admin.html';
                            });
                    }
                };
            }
        };
    }]);
    app.directive('dirManageUsers', function () {
        return {
            require: 'accountCtrl',
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/account_manage_users_view.html',
            controller: 'accountAdminCtrl'

        };
    });
    app.directive('dirDialogWindow', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/dialog_window.html'
            //controller: 'accountAdminCtrl'
        };
    });
    app.directive('dirRolePermissions', ['$http', '$rootScope', function ($http, $rootScope) {
        return {
            restrict: 'E',
            transclude: true,
            template: '<ng-include src="tmpl"/>',
            scope: '=',
            compile: function (tElem, tAttrs) {
                return {
                    pre: function preLink(scope, iElement, iAttrs, controller) {
                        scope.tmpl = 'views/directives/account_role_perm_user.html';
                    },
                    post: function postLink(scope, iElement, iAttrs) {
                        // pass user token to Web API
                        $http({
                            method: 'POST',
                            url: api + '/account/getuserperm',
                            data: $rootScope.User.Login,
                            headers: { 'Content-Type': 'application/json' }
                        })
                            .then(function (response) {
                                // change the template based on User privilege
                                scope.perms = response.data;
                                if (scope.perms.EditFF === true && scope.perms.EditSA === true)
                                    scope.tmpl = 'views/directives/account_role_perm_sa_ff.html';
                                else if (scope.perms.EditSA === true)
                                    scope.tmpl = 'views/directives/account_role_perm_sa.html';
                                else if (scope.perms.EditAdmin === true)
                                    scope.tmpl = 'views/directives/account_role_perm_admin.html';
                            });
                    }
                };
            }
        };
    }]);
    app.directive('dirDashboardData', ['$rootScope', function ($rootScope) {
        return {
            restrict: 'E',
            transclude: true,
            template: '<ng-include src="tmpl"/>',
            compile: function (tElem, tAttrs) {
                return {
                    post: function postLink(scope, iElement, iAttrs) {
                        // check the user's company type and load corresponding dashboard view
                        switch ($rootScope.User.Company.CompanyTypeID) {
                            case 4: // Custom Clearing
                                scope.tmpl = 'views/directives/dashboard_cc.html'; break;
                            case 5: // Freight Forwarder
                                scope.tmpl = 'views/directives/dashboard_ff.html'; break;
                            case 6: // Consignee
                                scope.tmpl = 'views/directives/dashboard_consignee.html'; break;
                            case 99: // Maritime
                                scope.tmpl = 'views/directives/dashboard_maritime.html'; break;
                            case 101: // National Bank
                                scope.tmpl = 'views/directives/dashboard_bank.html'; break;
                            case 102: // National Customs
                                scope.tmpl = 'views/directives/dashboard_customs.html'; break;
                        }

                    }
                };
            }
        };
    }]);

    // dailyActivities dir index
    app.directive('dirDailyActivity', function () {
        return {         
            restrict: 'E',
            transclude: true,            
            templateUrl: 'views/directives/activity/dailyactivity.html'
        };
    });

    // breakbulk dir
    app.directive('dirBreakBulk', function () {
        return {
            require: 'indexBreakBulkCtrl',
            restrict: 'E',
            transclude: true,
            controller: 'activityController',
            templateUrl: 'views/directives/activity/break_bulk.html'
        };
    });
    app.directive('dirBreakBulkWindow', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/break_bulk_preview.html'
        };
    });
    app.directive('dirOptionsMobileActivity', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/options_mobile_activity.html'
        };
    });

    // multimodal dir
    app.directive('dirMultiModal', function () {
        return {
            require: 'indexMultiModalCtrl',
            restrict: 'E',
            transclude: true,
            controller: 'activityController',
            templateUrl: 'views/directives/activity/multi_modal.html'
        };
    });
    app.directive('dirMultiModalWindow', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/multi_modal_preview.html'
        };
    });
    app.directive('dirOptionsMobileMultiActivity', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/options_mobile_multi_activity.html'
        };
    });

    // unimodal dir
    app.directive('dirUniModal', function () {
        return {
            require: 'indexUniModalCtrl',
            restrict: 'E',
            transclude: true,
            controller: 'activityController',
            templateUrl: 'views/directives/activity/uni_modal.html'
        };
    });
    app.directive('dirUniModalWindow', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/uni_modal_preview.html'
        };
    });
    app.directive('dirOptionsMobileUniActivity', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/options_mobile_uni_activity.html'
        };
    });

    app.directive('stringToNumber', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                ngModel.$parsers.push(function (value) {
                    return '' + value;
                });
                ngModel.$formatters.push(function (value) {
                    return parseFloat(value);
                });
            }
        };
    });
    app.directive('renderNameFor', ['$rootScope', function ($rootScope) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {

                var motObj = undefined;
                if ($rootScope.mots) {
                    for (var i in $rootScope.mots) {
                        if (attrs.renderNameFor !== '' && attrs.renderNameFor === $rootScope.mots[i].ID.toString()) {
                            motObj = $rootScope.mots[i];
                            break;
                        }
                    }
                }

                if (motObj !== undefined) {
                    switch (motObj.Mode.toLowerCase()) {
                        case "air":
                            attrs.i18n = '_AirWayBill_';
                            break;
                        default:
                            attrs.i18n = '_TruckWayBill_';
                            break;
                    }
                }
            }
        };
    }]);
    app.directive('autoComplete', ['$parse', 'autoCompleteDataService', function ($parse, autoCompleteDataService) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs, ctrl) {
                $(element).autocomplete({
                    source: autoCompleteDataService.getSource(attrs.source, attrs.extraparams), // from service                    
                    minLength: 2,
                    delay: 500,
                    classes: {
                        "ui-autocomplete": "highlight"
                    },
                    select: function (event, selectedItem) {
                        scope.$apply(function () {
                            $parse(attrs.ngModel).assign(scope, selectedItem.item.value);
                        });
                    }
                });
            }
        };
    }]);

    app.directive("datepickerFrom", ['$parse', function ($parse) {
        return {
            restrict: "A",
            // responsible for registering DOM listeners as well as updating the DOM
            link: function (scope, element, attrs) {
                $(element)
                    .datepicker({
                        defaultDate: "",
                        changeMonth: true,
                        changeYear: true,
                        numberOfMonths: 1
                    })
                    .on("change", function () {
                        $('#ToDate').datepicker("option", "minDate", getDate(this));

                        // bind the date
                        dateVal = this.value;
                        scope.$apply(function () {
                            $parse(attrs.ngModel).assign(scope, dateVal);
                        });
                    });

                scope.$on("$destroy", function () {
                    element.off();
                });

                var dateFormat = "mm/dd/yy";
                function getDate(element) {
                    var date;
                    try {
                        date = $.datepicker.parseDate(dateFormat, element.value);
                    } catch (error) {
                        date = null;
                    }

                    return date;
                }
            }
        };
    }]);
    app.directive("datepickerTo", ['$parse', function ($parse) {
        return {
            restrict: "A",
            // responsible for registering DOM listeners as well as updating the DOM
            link: function (scope, element, attrs) {
                $(element)
                    .datepicker({
                        defaultDate: "",
                        changeMonth: true,
                        changeYear: true,
                        numberOfMonths: 1
                    })
                    .on("change", function () {
                        $('#FromDate').datepicker("option", "maxDate", getDate(this));

                        // bind the date
                        dateVal = this.value;
                        scope.$apply(function () {
                            $parse(attrs.ngModel).assign(scope, dateVal);
                        });
                    });

                scope.$on("$destroy", function () {
                    element.off();
                });

                var dateFormat = "mm/dd/yy";
                function getDate(element) {
                    var date;
                    try {
                        date = $.datepicker.parseDate(dateFormat, element.value);
                    } catch (error) {
                        date = null;
                    }

                    return date;
                }
            }
        };
    }]);

    app.directive('rptFilterDate', function () {
        return {
            restrict: 'E',
            templateUrl: 'views/reports/filters/rpt_filter_date.html'
        };
    });
    app.directive('rptFilterDateTin', function () {
        return {
            restrict: 'E',
            templateUrl: 'views/reports/filters/rpt_filter_date_tin.html'
        };
    });
    app.directive('rptFilterDateCountry', function () {
        return {
            restrict: 'E',
            templateUrl: 'views/reports/filters/rpt_filter_date_country.html'
        };
    });
    app.directive('rptFilterDateBill', function () {
        return {
            restrict: 'E',
            templateUrl: 'views/reports/filters/rpt_filter_date_bill.html'
        };
    });
    app.directive('rptFilterDateTinBill', function () {
        return {
            restrict: 'E',
            templateUrl: 'views/reports/filters/rpt_filter_date_tin_bill.html'
        };
    });
    app.directive('rptFilterDateTinCargo', function () {
        return {
            restrict: 'E',
            templateUrl: 'views/reports/filters/rpt_filter_date_tin_cargo.html'
        };
    });
    app.directive('rptFilterTinCargoCountry', function () {
        return {
            restrict: 'E',
            templateUrl: 'views/reports/filters/rpt_filter_tin_cargo_country.html'
        };
    });
    app.directive('rptFilterDateTinProblem', function () {
        return {
            restrict: 'E',
            templateUrl: 'views/reports/filters/rpt_filter_date_tin_problem.html'
        };
    });
    app.directive('rptFilterTinProblem', function () {
        return {
            restrict: 'E',
            templateUrl: 'views/reports/filters/rpt_filter_tin_problem.html'
        };
    });

})();
(function () {
    app.directive('dirDatepicker', function () {
        return {
            restrict: 'AE',
            scope: {
                dt: '='
            },
            transclude: true,
            templateUrl: 'views/datepicker-date.html',
            controller: 'datepickerPopupController',
            link: function (scope, elem, attrs) {
                if (attrs.maxDate && attrs.maxDate === 'null') scope.dateOptions.maxDate = null;
                else scope.dateOptions.maxDate = new Date();

                if (attrs.minDate && (attrs.minDate !== 'null' || attrs.minDate !== undefined))
                    scope.dateOptions.minDate = new Date(attrs.minDate);
                else scope.dateOptions.minDate = null;

                if (attrs.required !== undefined || attrs.required === 'true') scope.required = true;
                else scope.required = false;
            }
        };
    });

})();
//cargoCanalApp.filter('unsafe', function ($sce) {
//    return function (val) {
//        return $sce.trustAsHtml(val);
//    };
//});
app.filter('groupByDate', function () {
    return function (dataArr, field) {
        const groupedObj = dataArr.reduce((previousVal, currentVal) => {
            const [year, month, day] = currentVal[field].split('-');
            const currentDate = new Date(year, month - 1, day.substr(0, day.indexOf('T'))).toJSON();

            if (!previousVal[currentDate]) {
                previousVal[currentDate] = [currentVal];
            } else {
                previousVal[currentDate].push(currentVal);
            }
            return previousVal;
        }, {});

        return Object.keys(groupedObj).map(key => ({ key, value: groupedObj[key] }));
    }
});
app.filter('groupByField', function () {
    return function (o, field) {
        var filtered = [];
        var groups = [];

        if (o !== undefined && o.length > 0 && field !== '') {
            for (var i in o) {
                if (groups.indexOf(o[i][field]) === -1) {
                    groups.push(o[i][field]);
                }
            }

            groups.sort();

            for (var g in groups) {
                filtered.push([]);

                for (var j in o) {
                    if (o[j][field] === groups[g]) {
                        filtered[g].push(o[j]);
                    }
                }
            }
        }
        else {
            filtered = o;
        }

        return filtered;
    }
});

app.filter('numberparse', function () {
    return function (number) {
        if (number === undefined || number === null || number < 1000) return number;

        let numerator = number;
        let denominator = 1;
        let remainder = 0;
        let numberString = '';
        const thousand = 1000;
        const million = Math.pow(thousand, 2);
        const billion = Math.pow(thousand, 3);
        const trillion = Math.pow(thousand, 4);

        while ((numerator * denominator >= denominator * thousand)) {
            denominator *= thousand;
            if (denominator > trillion) break;
            numerator = Math.floor(number / denominator);
            remainder = number % denominator;
        }

        const remDiv = (remainder / denominator).toString();
        const remStr = remDiv.substr(remDiv.indexOf('.') + 1, 1);
        const kText = ' K';
        const mText = ' M';
        const bText = ' B';
        const tText = ' T';

        numberString = numerator;

        switch (denominator) {
            case thousand:
                numberString += (remStr === '0' ? kText : '.' + remStr + kText);
                break;
            case million:
                numberString += (remStr === '0' ? mText : '.' + remStr + mText);
                break;
            case billion:
                numberString += (remStr === '0' ? bText : '.' + remStr + bText);
                break;
            case trillion:
                numberString += (remStr === '0' ? tText : '.' + remStr + tText);
                break;
            default:
                numberString = '999' + tText + '+';
        }
        
        return numberString;
    }
});
app.filter('statusavail', function () {
    return function (obj, ie) {
        if (obj === undefined || obj === null) return;

        var objCol = [];
        for (var i in obj) {
            if (ie === 1 && (obj[i].ImpExpTypeID === 1 || obj[i].ImpExpTypeID === 3)) {
                objCol.push(obj[i]);
            }

            if (ie === 2 && (obj[i].ImpExpTypeID === 2 || obj[i].ImpExpTypeID === 3)) {
                objCol.push(obj[i]);
            }
        }

        return objCol;
    }
});
app.filter('startFrom', function () {
    return function (input, start) {
        if (input) {
            start = +start; //parse to int
            return input.slice(start);
        }
        return [];
    }
});


app.filter('carrier', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.carriers) {
            if ($rootScope.carriers[i].ID == id) {
                return $rootScope.carriers[i].CarrierName;
            }
        }
    };
}]);
app.filter('country', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.countries) {
            if ($rootScope.countries[i].ID == id) {
                return $rootScope.countries[i].Name;
            }
        }
    };
}]);
app.filter('cargo', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.cargos) {
            if ($rootScope.cargos[i].ID == id) {
                return $rootScope.cargos[i].CargoName;
            }
        }
    };
}]);
app.filter('subcargo', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.subcargos) {
            if ($rootScope.subcargos[i].ID == id) {
                return $rootScope.subcargos[i].Description;
            }
        }
    };
}]);
app.filter('danger', function () {
    return function (Dangerous) {
        switch (Dangerous) {
            case true: return '***';
            default: return ''
        }
    };
});
app.filter('resolvedtext', function () {
    return function (resolved) {
        switch (resolved) {
            case true: return 'Resolved';
            default: return 'Pending'
        }
    };
});
app.filter('inco', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.incoTerms) {
            if ($rootScope.incoTerms[i].ID == id) {
                return $rootScope.incoTerms[i].IncoName;
            }
        }
    };
}]);
app.filter('location', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.locations) {
            if ($rootScope.locations[i].ID == id) {
                return $rootScope.locations[i].LocationName;
            }
        }
    };
}]);
app.filter('mot', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.mots) {
            if ($rootScope.mots[i].ID == id) {
                return $rootScope.mots[i].Mode;
            }
        }
    };
}]);
app.filter('podName', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.pods) {
            if ($rootScope.pods[i].ID == id) {
                return $rootScope.pods[i].PortName;
            }
        }
    };
}]);
app.filter('polName', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.pols) {
            if ($rootScope.pols[i].ID == id) {
                return $rootScope.pols[i].PortName;
            }
        }
    };
}]);
app.filter('reason', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.importExportReasons) {
            if ($rootScope.importExportReasons[i].ID == id) {
                return $rootScope.importExportReasons[i].Reason;
            }
        }
    };
}]);

app.filter('stuffMode', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.stuffModes) {
            if ($rootScope.stuffModes[i].ID == id) {
                return $rootScope.stuffModes[i].Description;
            }
        }
    };
}]);
app.filter('unit', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.units) {
            if ($rootScope.units[i].ID == id) {
                return $rootScope.units[i].UnitName;
            }
        }
    };
}]);
app.filter('vessel', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.carrierVessels) {
            if ($rootScope.carrierVessels[i].ID == id) {
                return $rootScope.carrierVessels[i].Name;
            }
        }
    };
}]);


app.filter('problem', function () {
    return function (ProblemID) {
        for (var i = 0; i < problemsList.length; i++) {
            if (problemsList[i].ID == ProblemID) {
                return problemsList[i].Problem;
            }
        }
    };
});


app.filter('statusName', function () {
    return function (StatusID) {
        for (var i = 0; i < statusList.length; i++) {
            if (statusList[i].ID == StatusID) {
                return statusList[i].Description;
            }
        }
    };
});

app.filter('completed', function () {
    return function (Completed) {
        switch (Completed) {
            case false:
                return '_Pending_';
            case true:
                return '_Done_';
        }
    };
});

app.filter('resolved', function () {
    return function (IsResolved) {
        switch (IsResolved) {
            case false:
                return 'N';
            case true:
                return 'Y';
        }
    };
});
(function () {
    "use strict";

    var uriReports = api + '/Reports';

    app
        .directive('mappingDirective', function () {

            var mappingDirectiveController = ['$scope', '$http', '$state', function ($scope, $http, $state) {

                //------------------------------------- M A P P I N G   O F   C O L U M N S ---------------

                //declaring variables
                $scope.flgAddNewMapping = false;
                $scope.rptCtrlWorking = false;
                $scope.mappingStatusMessage = "";
                $scope.mappingsCollection = [];

                //when Add New button is clicked to add new column mapping
                $scope.setAddedColumnMapping = function () {
                    $scope.flgAddNewMapping = true;
                    $scope.mappingStatusMessage = "";
                };

                //saving mapping
                $scope.saveMapping = function () {

                    $scope.rptCtrlWorking = true;
                    $scope.mappingStatusMessage = "Saving mapping....";

                    // initialize the first 5 columns if none was set
                    if ($scope.currentColumnMapping.ColumnMappings === undefined
                        || $scope.currentColumnMapping.ColumnMappings.length === 0) {
                        $scope.currentColumnMapping.ColumnMappings = [true, true, true, true, true];
                    }

                    //call the AddMapping in ReportsController
                    $http.post(uriReports + '/AddMapping', $scope.currentColumnMapping)
                        .then(function (response) {
                            $scope.rptCtrlWorking = false;
                            $scope.mappingStatusMessage = "Mapping '" + $scope.currentColumnMapping.Name + "' saved.";
                            $scope.getMappings();

                            //show the add New Mapping button again
                            $scope.flgAddNewMapping = false;
                        }, function (errorResponse) {
                            $scope.mappingStatusMessage = "Oops! Unable to save mapping '" + $scope.currentColumnMapping.Name + "'.";
                            $scope.rptCtrlWorking = false;
                        });
                };

                // set all checkboxes for $scope.currentColumnMapping.ColumnMappings
                $scope.toggleCurrentMapping = function (bool) {

                    // set boolean value based on customReportHeaders
                    $scope.currentColumnMapping.ColumnMappings = [];

                    for (var i = 0; i < $scope.customReportHeaders.length; i++) {
                        $scope.currentColumnMapping.ColumnMappings.push(bool);
                    }
                };

                $scope.refreshTableColumns = function () {
                    $scope.tableColumns = [];
                    for (var i = 0; i < $scope.currentColumnMapping.MappingModelParams.length; i++) {
                        $scope.tableColumns.push(''); // just push in something to create an index
                    }
                };

                //get all mappings
                $scope.getMappings = function () {
                    $http({
                        method: 'GET',
                        url: uriReports + '/GetMappings',
                        headers: { 'Content-Type': 'application/json' }
                    })
                        .then(function (response) {
                            $scope.mappingsCollection = response.data;
                        });
                };

                $scope.getMappings();

                //--------------- E N D   O F  C O L U M N   M A P P I N G   CODE---------

                //-------------PARAMS LIST ARRAY FOR MAPPINGMODELPARAMS --------
                $scope.currentParamsList = [];
                $scope.addParams = function (lOp, tbl, col, op, v) {
                    var mmParams = {};
                    mmParams.TableName = tbl;
                    mmParams.ColumnName = col;
                    mmParams.Op = op;
                    mmParams.Lop = lOp;
                    mmParams.Val = v;
                    mmParams.IndexPosition = $scope.currentParamsList.lenth;
                    mmParams.Id = $scope.currentParamsList.length;
                    mmParams.MappingModelId = "";

                    $scope.currentParamsList.push(mmParams);
                };

                $scope.resetParams = function () {
                    $scope.currentParamsList = [];
                };
            }];

            return {
                restrict: 'EA', //Default in 1.3+
                //note scope: is removed to use the parent controller scope
                controller: mappingDirectiveController,
                templateUrl: 'templates_quarantined/common/view-dir-column-mapping.html'
            };
        })
        .directive('customReportsQueryBuilder', function () {
            return {
                restrict: 'EA', //Default in 1.3+
                //note scope: is removed to use the parent controller scope
                templateUrl: 'templates_quarantined/common/view-dir-custom-reports-query-builder.html'
            };
        });
}());
angular.module("chartjsAngular", [])
    .factory('chartjsFactory', function () {
        var _ctx = '';
        var service = {};

        service.setCanvas = function (ctx) {
            return _ctx = document.getElementById(ctx);
        };

        service.createChart = function (_type, _data, _options) {
            // takes care of overlapping points
            //Chart.Controller.prototype.getElementsAtEvent = function (e) {
            //    var helpers = Chart.helpers;
            //    var eventPosition = helpers.getRelativePosition(e, this.chart);
            //    var elementsArray = [];

            //    var found = (function () {
            //        if (this.data.datasets) {
            //            for (var i = 0; i < this.data.datasets.length; i++) {
            //                if (helpers.isDatasetVisible(this.data.datasets[i])) {
            //                    for (var j = 0; j < this.data.datasets[i].metaData.length; j++) {
            //                        if (this.data.datasets[i].metaData[j].inLabelRange(eventPosition.x, eventPosition.y)) {
            //                            return this.data.datasets[i].metaData[j];
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }).call(this);

            //    if (!found) {
            //        return elementsArray;
            //    }

            //    helpers.each(this.data.datasets, function (dataset, dsIndex) {
            //        if (helpers.isDatasetVisible(dataset)) {
            //            elementsArray.push(dataset.metaData[found._index]);
            //        }
            //    });

            //    return elementsArray;
            //};

            return new Chart(_ctx,
                {
                    type: _type,
                    data: _data,
                    options: _options
                });
        };

        return service;
    })
    .provider('chartjsGlobalProvider', function () {
        this.$get = function () {
            return {
                setGlobal: function (item, prop, value) {
                    Chart.defaults.global.elements[item][prop] = value;
                }
            };
        };
    });