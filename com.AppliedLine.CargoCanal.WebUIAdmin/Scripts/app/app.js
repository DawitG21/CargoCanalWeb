var app = angular.module('app', ['ui.router', 'ngStorage', 'ngSanitize']);

app.config(['$stateProvider', '$urlRouterProvider', '$locationProvider', function ($stateProvider, $urlRouterProvider, $locationProvider) {
    $locationProvider.hashPrefix(''); // remove annoying !
    $locationProvider.html5Mode(true); // pretty url with web.config set
    // catch all route
    // send users to the home page
    $urlRouterProvider.otherwise('/home');

    $stateProvider.state('home', {
        url: '/home',
        templateUrl: 'views/home.html',
        controller: 'mainController'
    });

    $stateProvider.state('login', {
        url: '/login',
        templateUrl: 'views/login.html',
        controller: 'loginController'
    });

    $stateProvider.state('termsofservice', {
        url: '/terms-of-service',
        templateUrl: 'views/terms_of_service.html'
    });

    $stateProvider.state('privacypolicy', {
        url: '/privacy-policy',
        templateUrl: 'views/privacy_policy.html'
    });

    $stateProvider.state('manager', {
        url: '/manager',
        templateUrl: 'views/manager.html',
        controller: 'managerCtrl'
    });
    $stateProvider.state('manager.addcompany', {
        url: '/addcompany',
        templateUrl: 'views/create_company.html'
    });
    $stateProvider.state('manager.companyservices', {
        url: '/companyservices',
        templateUrl: 'views/view_company_services.html'
    });

    $stateProvider.state('manager.addcarrier', {
        url: '/addcarrier',
        templateUrl: 'views/create_carrier.html'
    });

    $stateProvider.state('manager.addport', {
        url: '/addport',
        templateUrl: 'views/create_port.html'
    });

    $stateProvider.state('manager.addvessel', {
        url: '/addvessel',
        templateUrl: 'views/create_vessel.html'
    });

    $stateProvider.state('manager.addlocation', {
        url: '/addlocation',
        templateUrl: 'views/create_location.html'
    });

}]);