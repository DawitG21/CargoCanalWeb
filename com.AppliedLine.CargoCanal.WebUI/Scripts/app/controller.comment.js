﻿(function () {
    'use strict';

    app.controller('commentCtrl', ['$scope', '$rootScope', '$state', 'appFactory', 'signalRHubProxy', 'commentService',
        function ($scope, $rootScope, $state, appFactory, signalRHubProxy, commentService) {

            let maxLength = 1000;    // maximum characters allowed as comment
            $scope.comments = [];
            $scope.comment = { CommentText: '' };


            // returns the total number of characters remaining
            $scope.getXtersLeft = function () {
                $scope.xtersLeft = appFactory.xtersLeft(maxLength, $scope.comment.CommentText.length);
            };


            // clear comment text and reset maximum xters left
            $scope.clearComment = function () {
                $scope.comment.CommentText = '';
                $scope.getXtersLeft();
            };


            $scope.clearComment();  // init xtersLeft and comment text

            $scope.closeWindow = function () {
                $scope.clearComment();
                $rootScope.showComment = false;
                appFactory.setModalOpen(false);
            };


            // listen for open comment
            $scope.$on('commentOpen', function (event, id) {
                $scope.comment.ImportExportID = parseInt(id);

                // get importExport comments thru service
                commentService.getComments(id)
                    .then(function (data) {
                        $scope.comments = data;
                    },
                    function (err) {
                        // handle error
                    });
            });


            $scope.submit = function () {
                // post comment and clear the comment box
                commentService.submitComment($scope.comment)
                    .then(function (data) {
                        $scope.clearComment();
                    }, function (err) {
                        appFactory.showDialog('Oops! Something went wrong.', true);
                    });
            };



            // SIGNALR CODES
            let commentProxy = signalRHubProxy(serverUrl, 'commentHub');
            commentProxy.on('commentAdded',
                function (comment) {
                    // add comment to matching importExport
                    if (comment.ImportExportID === $scope.comment.ImportExportID) {
                        $scope.comments.push(comment);
                    }
                });

        }]);

})();