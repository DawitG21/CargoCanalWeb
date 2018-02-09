var app = angular.module('app', ['localization', 'ngAnimate', 'ui.router', 'ui.bootstrap', 'ngStorage', 'ngMessages']);

app.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

    // catch all route
    // send users to the home page
    $urlRouterProvider.otherwise('/home');

    $stateProvider
        //
        .state('home', {
            url: '/home',
            templateUrl: 'templates/home.min.html',
            controller: 'appController'
        })

        .state('home.tracking', {
            url: '/tracking',
            templateUrl: 'templates/tracking/tracking.min.html',
            controller: 'trackingController'
        })

        .state('config', {
            url: '/config',
            templateUrl: 'templates/admin/config.min.html',
            controller: 'configController'
        })

        .state('registeradmin', {
            url: '/registeradmin',
            templateUrl: 'templates/register/registeradmin.min.html',
            controller: 'registerController'
        })
            .state('registeradmin.profile', {
                url: '/profile',
                templateUrl: 'templates/register/registeradmin-profile.min.html'
            })

        .state('registercustomsclearing', {
            url: '/registercc',
            templateUrl: 'templates/register/registercustomsclearing.min.html',
            controller: 'registerController'
        })
            .state('registercustomsclearing.profile', {
                url: '/profile',
                templateUrl: 'templates/register/registercustomsclearing-profile.min.html'
            })

        .state('registercustomer', {
            url: '/registercustomer',
            templateUrl: 'templates/register/registercustomer.min.html',
            controller: 'registerController'
        })
            .state('registercustomer.profile', {
                url: '/profile',
                templateUrl: 'templates/register/registercustomer-profile.min.html'
            })

        .state('login', {
            url: '/login',
            templateUrl: 'templates/login/login.min.html',
            controller: 'loginController'
        })
        
        .state('myaccount', {
            url: '/myaccount',
            templateUrl: 'templates/account/view-account.min.html',
            controller: 'myAccountController'
        })

        .state('customeraccount', {
            url: '/customer',
            templateUrl: 'templates/account/view-customeraccount.min.html',
            controller: 'myAccountController'
        })

        .state('registertype', {
            url: '/registrationtype',
            templateUrl: 'templates/register/registertype.min.html',
            controller: 'registerController'
        })

        .state('register', {
            url: '/register',
            templateUrl: 'templates/register/register.min.html',
            controller: 'registerController'
        })
            .state('register.profile', {
                url: '/profile',
                templateUrl: 'templates/register/register-profile.min.html'
            })
            .state('register.company', {
                url: '/company',
                templateUrl: 'templates/register/register-company.min.html'
            })

        

        .state('tracking', {
            url: '/tracking',
            templateUrl: 'templates/tracking/tracking.min.html',
            controller: 'trackingController'
        })

        .state('dashboard', {
            url: '/dashboard',
            templateUrl: 'templates/dashboard.min.html',
            controller: 'dashboardController'
        })

            .state('dashboard.customsclearance', {
                url: '/cc',
                templateUrl: 'templates/customs/customs-clearance.min.html'
            })

            .state('dashboard.customsdispatch', {
                url: '/cd',
                templateUrl: 'templates/customs/customs-dispatch.min.html'
            })

            .state('dashboard.customsinspection', {
                url: '/ci',
                templateUrl: 'templates/customs/customs-inspection.min.html'
            })

            .state('dashboard.tracking', {
                url: '/agenttracking',
                templateUrl: 'templates/tracking/agent-tracking.min.html'
            })

            .state('dashboard.imports', {
                url: '/imports',
                templateUrl: 'templates/import/dashboard-imports.min.html'
            })

                .state('dashboard.imports.viewimport', {
                    url: '/viewimport',
                    templateUrl: 'templates/import/view-import.min.html'
                })

                    .state('dashboard.imports.viewimport.terminate', {
                        url: '/terminate',
                        templateUrl: 'templates/importexport/terminateimportexport.min.html'
                    })

                    .state('dashboard.imports.viewimport.completed', {
                        url: '/completed',
                        templateUrl: 'templates/importexport/completedimportexport.min.html'
                    })

                .state('dashboard.imports.status', {
                    url: '/status',
                    templateUrl: 'templates/importexport/dashboard-ie-status.min.html'
                })

                .state('dashboard.imports.problem', {
                    url: '/problem',
                    templateUrl: 'templates/importexport/dashboard-ie-problem.min.html'
                })

                .state('dashboard.imports.closeproblem', {
                    url: '/closeproblem',
                    templateUrl: 'templates/importexport/dashboard-ie-problem-close.min.html'
                })

                .state('dashboard.imports.add', {
                    url: '/add',
                    templateUrl: 'templates/importexport/dashboard-ie-add.min.html',
                    controller: 'importExportController'
                })

                    .state('dashboard.imports.add.bill', {
                        url: '/bill',
                        templateUrl: 'templates/import/dashboard-imports-add-bill.min.html'
                    })

                    .state('dashboard.imports.add.carrier', {
                        url: '/carrier',
                        templateUrl: 'templates/importexport/dashboard-ie-add-carrier.min.html'
                    })

                    .state('dashboard.imports.add.item', {
                        url: '/item',
                        templateUrl: 'templates/importexport/dashboard-ie-add-item.min.html'
                    })

                    .state('dashboard.imports.add.itemdetail', {
                        url: '/itemdetail',
                        templateUrl: 'templates/importexport/dashboard-ie-add-itemdetail.min.html'
                    })

                    .state('dashboard.imports.add.cost', {
                        url: '/cost',
                        templateUrl: 'templates/importexport/dashboard-ie-add-cost.min.html'
                    })
                    .state('dashboard.imports.add.tracking', {
                        url: '/tracking',
                        templateUrl: 'templates/importexport/dashboard-ie-add-tracking.min.html'
                    })

            .state('dashboard.exports', {
                url: '/exports',
                templateUrl: 'templates/export/dashboard-exports.min.html'
            })

                .state('dashboard.exports.viewexport', {
                    url: '/viewexport',
                    templateUrl: 'templates/export/view-export.min.html'
                })

                    .state('dashboard.exports.viewexport.terminate', {
                        url: '/terminate',
                        templateUrl: 'templates/importexport/terminateimportexport.min.html'
                    })

                    .state('dashboard.exports.viewexport.completed', {
                        url: '/completed',
                        templateUrl: 'templates/importexport/completedimportexport.min.html'
                    })

                .state('dashboard.exports.status', {
                    url: '/status',
                    templateUrl: 'templates/importexport/dashboard-ie-status.min.html'
                })

                .state('dashboard.exports.problem', {
                    url: '/problem',
                    templateUrl: 'templates/importexport/dashboard-ie-problem.min.html'
                })

                .state('dashboard.exports.closeproblem', {
                    url: '/closeproblem',
                    templateUrl: 'templates/importexport/dashboard-ie-problem-close.min.html'
                })

                .state('dashboard.exports.add', {
                    url: '/add',
                    templateUrl: 'templates/importexport/dashboard-ie-add.min.html',
                    controller: 'importExportController'
                })

                    .state('dashboard.exports.add.bill', {
                        url: '/bill',
                        templateUrl: 'templates/export/dashboard-exports-add-bill.min.html'
                    })

                    .state('dashboard.exports.add.bill1', {
                        url: '/bill1',
                        templateUrl: 'templates/export/dashboard-exports-add-bill1.min.html'
                    })

                    .state('dashboard.exports.add.carrier', {
                        url: '/carrier',
                        templateUrl: 'templates/importexport/dashboard-ie-add-carrier.min.html'
                    })

                    .state('dashboard.exports.add.item', {
                        url: '/item',
                        templateUrl: 'templates/importexport/dashboard-ie-add-item.min.html'
                    })

                    .state('dashboard.exports.add.itemdetail', {
                        url: '/itemdetail',
                        templateUrl: 'templates/importexport/dashboard-ie-add-itemdetail.min.html'
                    })

                    .state('dashboard.exports.add.cost', {
                        url: '/cost',
                        templateUrl: 'templates/importexport/dashboard-ie-add-cost.min.html'
                    })
                    .state('dashboard.exports.add.tracking', {
                        url: '/tracking',
                        templateUrl: 'templates/importexport/dashboard-ie-add-tracking.min.html'
                    })

            .state('dashboard.attachments', {
                url: '/attachments',
                templateUrl: 'templates/report/dashboard-attachments.min.html',
                controller: 'attachmentsController'
            })
            .state('dashboard.attachments.emails', {
                url: '/emails',
                templateUrl: 'templates/report/dashboard-attachments-emails.min.html'
            })

            .state('dashboard.customreports', {
                url: '/customreports',
                templateUrl: 'templates/report/dashboard-customreports.min.html',
                controller: 'reportsController'
            })

            .state('dashboard.reports', {
                url: '/reports',
                templateUrl: 'templates/report/dashboard-reports.min.html',
                controller: 'reportsController'
            })

            .state('dashboard.reports.statusSummaryReport', {
                url: '/summary',
                templateUrl: 'templates/report/report-statusSummary.min.html'
            })

            .state('dashboard.reports.pendingReport', {
                url: '/pending',
                templateUrl: 'templates/report/report-pending.min.html'
            })

            .state('dashboard.reports.problemsReport', {
                url: '/problems',
                templateUrl: 'templates/report/report-problems.min.html'
            })

            .state('dashboard.reports.demurrageReport', {
                url: '/demurrage',
                templateUrl: 'templates/report/report-demurrage.min.html'
            })

            .state('dashboard.reports.cargoWeightReport', {
                url: '/cargoweight',
                templateUrl: 'templates/report/report-cargoweight.min.html'
            })

            .state('dashboard.reports.cargoInTransitWeightReport', {
                url: '/cargointransitweight',
                templateUrl: 'templates/report/report-cargointransitweight.min.html'
            })

            .state('dashboard.reports.transitTimeReport', {
                url: '/transittime',
                templateUrl: 'templates/report/report-transittime.min.html'
            })

            .state('dashboard.reports.bystatus', {
                url: '/bystatus',
                templateUrl: 'templates/report/report-bystatus.min.html'
            });

}]);