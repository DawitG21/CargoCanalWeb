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
                                    console.log('forwarders analytics', response.data);
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
                                            $scope.dashboard.forwarderActivity.data.data[i].length,
                                            //label: $scope.dashboard.forwarderActivity.data.series[i]
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