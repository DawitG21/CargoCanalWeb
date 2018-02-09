(function () {
    "use strict";

    app.factory("autoCompleteDataService", ["$http", function ($http) {
        return {
            getSource: function (srcLink) {
                // make a call to the external source link
                return api + '/values/' + srcLink;
            }
        }
    }]);
    app.service('refresher', ['$sessionStorage', "$rootScope", "$state", function ($sessionStorage, $rootScope, $state) {
        this.refreshApp = function () {

            if ($sessionStorage.__user == undefined) {
                //console.log('redirected to login');
            } else {
                $rootScope.User = $sessionStorage.__user;
            }

        };

        this.refreshApp();
    }]);

    app.factory('appFactory', ['$rootScope', '$state', '$http', '$q', '$sce', function ($rootScope, $state, $http, $q, $sce) {
        $rootScope.modalOpen = false;
        $rootScope.login = {};

        var _isTinValid = true;
        var service = {};
        var odataUrl = api.substring(0, api.indexOf('/api')) + '/odata';

        service.setModalOpen = function (bool) {
            $rootScope.modalOpen = bool;
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

        
        // initialize common functions used by multiple controllers
        let _initHelpers = function () {
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


            // optionsMore object helpers
            $rootScope.optionsMore = {
                openWindow: false,
                show: function (record, recordIndex) {
                    $rootScope.record = record;
                    $rootScope.recordIndex = recordIndex;
                    $rootScope.optionsMore.openWindow = true;
                    service.setModalOpen(true);
                },
                closeWindow: function () {
                    $rootScope.optionsMore.openWindow = false;
                    service.setModalOpen(false);
                    $rootScope.record = undefined;
                    $rootScope.recordIndex = undefined;
                }
            };
        };


        // set base64 image to user profile
        service.setDataImage = function () {
            switch ($rootScope.User.Person.Photo) {
                case null: case undefined: case '': break;
                default:
                    $rootScope.User.Person.PhotoDataUrl = 'data:image/jpg;base64,' + $rootScope.User.Person.Photo;
            }
        };


        // return an icon based on if active is true or false
        service.getActiveIcon = function (active) {
            if (active) return 'status-icon-green';
            return 'status-icon-red';
        };

        // sign in helper
        service.signIn = function () {
            return $http({
                method: 'POST',
                url: api + '/account/postlogin',
                data: $rootScope.login,
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            })
                .then(function (response) {
                    return response;
                }, function (error) { return error; });
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
                }, function (error) { return error; });
        };

        service.renewSubscription = function () {
            $rootScope.closeDialog();
            $http({
                method: 'PUT',
                url: api + '/account/PutSubscribe',
                data: $rootScope.serviceSelected,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    service.showDialog('Subscription renewed successfully.');
                    $rootScope.serviceSelected = undefined;
                }, function (error) {
                    service.showDialog('Subscription renewal failed.', true);
                    $rootScope.serviceSelected = undefined;
                });
            
        };


        // get companies
        service.getCompanies = function (searchText) {
            return $http({
                method: 'GET',
                url: api + '/account/getcompanies?company=' + searchText,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    return response.data;
                }, function (error) { return []; });

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


        // prep cards, dataTable
        service.prepCards = function () {
            setTimeout(function () {
                let elm = document.getElementById('cardsjs');
                if (elm !== undefined) elm.parentNode.removeChild(elm);

                let script = document.createElement('script');
                script.id = "cardsjs";
                script.src = "scripts/app/cards.js";
                script.async = false; // This is required for synchronous execution
                document.body.appendChild(script);
            }, 500);
        };

        _initHelpers();
        return service;
    }]);

})();