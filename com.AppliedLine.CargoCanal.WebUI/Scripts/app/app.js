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
        controller: 'indexmultiModalCtrl'
    });

    $stateProvider.state('multimodal.create', {
        url: '/create',
        templateUrl: 'views/activitymultimodal/create_multimodal.html',
        controller: 'createMultiModalCtrl'
    });

    $stateProvider.state('multimodal.edit', {
        url: '/edit',
        templateUrl: 'views/activitymultimodal/edit_multimodal.html',
        controller: 'editMultiModalCtrl'
    });

    ///
    $stateProvider.state('unimodal', {
        url: '/unimodal',
        templateUrl: 'views/activityunimodal/uni_modal_activity.html',
        controller: 'indexuniModalCtrl'
    });

    $stateProvider.state('unimodal.create', {
        url: '/createl',
        templateUrl: 'views/activityunimodal/create_unimodal.html',
        controller: 'createUniModalCtrl'
    });

    $stateProvider.state('unimodal.edit', {
        url: '/edit',
        templateUrl: 'views/activityunimodal/edit_unimodal.html',
        controller: 'editUniModalCtrl'
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