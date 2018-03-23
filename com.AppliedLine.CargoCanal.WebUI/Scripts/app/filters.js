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