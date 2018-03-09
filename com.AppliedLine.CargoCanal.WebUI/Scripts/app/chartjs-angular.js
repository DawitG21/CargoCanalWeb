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