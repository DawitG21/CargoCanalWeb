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