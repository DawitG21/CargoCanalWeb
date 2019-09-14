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