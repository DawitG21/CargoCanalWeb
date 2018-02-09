(function () {
    "use strict";

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

    app.factory('appFactory', ['$rootScope', '$state', '$http', '$q', function ($rootScope, $state, $http, $q) {
        $rootScope.modalOpen = false;
        var _isTinValid = true;
        var service = {};

        service.setModalOpen = function (bool) {
            $rootScope.modalOpen = bool;
        };

        // set base64 image to user profile
        service.setDataImage = function () {
            switch ($rootScope.User.Person.Photo) {
                case null: case undefined: case '': break;
                default:
                    $rootScope.User.Person.PhotoDataUrl = 'data:image/jpg;base64,' + $rootScope.User.Person.Photo;
            }
        };

        // return a class that sets the <body> overflow: hidden
        service.getModalOpenClass = function () {
            if ($rootScope.modalOpen) return 'fullscreen-open';
            return '';
        };

        // return an icon based on if active is true or false
        service.getActiveIcon = function (active) {
            if (active) return 'status-icon-green';
            return 'status-icon-red';
        };


        // validate a Company TIN is registered
        service.validateTin = function (tin, state) {
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

        // get if the Company TIN is valid
        service.getIsTinValid = function () {
            return _isTinValid;
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
        this.dateoptions = {};
        this.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate', 'M!/d!/yyyy'];

        // pass this to dateDisabled property of datepickerProvider.dateoptions to disable weekends
        this.disableWeekends = function (data) {
            var date = data.date, mode = data.mode;
            return mode === 'day' && (date.getDay() === 0 || date.getDay() === 6);
        };

        // publicly available properties
        this.$get = function () {
            var that = this;
            return {
                getFormats: function () {
                    return that.formats;
                },
                getDateoptions: function () {
                    return that.dateoptions;
                },
                getDisableWeekends: function () {
                    return that.disableWeekends; // returns a function NOT FIRED
                }
            }
        };

    }]);


    // initialize the default datepicker options
    app.config(['datepickerProviderProvider', function (datepickerProviderProvider) {
        datepickerProviderProvider.dateoptions = {
            datepickerMode: 'day',
            formatYear: 'yyyy',
            showWeeks: false,
            dateDisabled: false,
            minDate: null,
            maxDate: new Date(),
            initDate: new Date()
        };
    }]);


    app.service('refresher', ['$sessionStorage', "$rootScope", function ($sessionStorage, $rootScope) {
        this.refreshApp = function () {

            if ($sessionStorage.__user === undefined) {
                //console.log('redirected to login');
            } else {
                $rootScope.User = $sessionStorage.__user;
            }

        };

        this.refreshApp();
    }]);

})();