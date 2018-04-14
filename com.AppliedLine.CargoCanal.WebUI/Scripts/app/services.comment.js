(function () {
    'use strict';


    app.service('commentService', ['$rootScope', '$http', function ($rootScope, $http) {
        let _url = '/comment';


        this.getComments = function (id) {
            return $http({
                method: 'GET',
                url: api + _url + '/getcomment/' + id,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    return response.data;
                }, function (error) {
                    throw new Error(error);
                });
        };

        // post a comment
        this.submitComment = function (comment) {
            // assign token
            comment.Token = $rootScope.User.Login.Token;

            return $http({
                method: 'POST',
                url: api + _url,
                data: comment,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    return response.data;
                }, function (error) {
                    throw new Error(error);
                });
        };

    }]);
})();