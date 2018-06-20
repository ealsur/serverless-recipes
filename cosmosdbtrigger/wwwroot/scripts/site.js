'use strict';

(function () {
    var connection = null;
    
    angular.module('serverless', [])
        .config(['$httpProvider', function ($httpProvider) {
            if (!$httpProvider.defaults.headers.get) {
                $httpProvider.defaults.headers.get = {};
            }
            $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Mon, 26 Jul 1997 05:00:00 GMT';
            $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
            $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
        }])
        .controller('Comments', function ($http, $timeout, $q, $rootScope) {
            var ctrl = this;
            ctrl.posts = [];

            var processMessages = function(messages){
                if (!messages) return;
                messages = JSON.parse(messages);
                for (var i = 0, len = messages.length; i < len; i++) {
                    var post = messages[i];
                    var feedbackedPost = ctrl.posts.find((post) => { return post.id === data.id });
                    feedbackedPost.score = response.data.score;
                    feedbackedPost.hasFeedback = true;
                }
            }

            connection.on('NewMessages', function(messages){
                $rootScope.$apply(function(){
                    processMessages(messages);
                });
            });

            $rootScope.$on('feedback', function (event, data) {
               
            });

            $rootScope.$on('message', function (event, data) {
                ctrl.posts.push({
                    id: data.id,
                    message: data.message,
                    hasFeedback: false,
                    score: 0
                });
            });
        })
        .controller('Poster', function ($http, $timeout, $q, $rootScope) {
            var ctrl = this;

            ctrl.message = '';
            ctrl.loading = false;
            ctrl.post = function () {
                ctrl.loading = true;
                $http({
                    method: 'POST',
                    url: '/api/save',
                    data: {
                        message: ctrl.message
                    }
                }).then(function (response) {
                    ctrl.loading = false;
                    $rootScope.$emit('message', {
                        message: ctrl.message,
                        id: response.data
                    });
                    ctrl.message = '';
                });
            };
        });

        
        $.get("/api/config", {},function (data) {
            connection = new signalR.HubConnectionBuilder()
                .withUrl(data.hubUrl, { accessTokenFactory: () => data.accessToken })
                .build();
            connection.start()
                .then(function () {
                     angular.bootstrap(document.body, ['serverless']);
                })
                .catch(function (error) {
                    console.error(error.message);
                });

        }, 'json');
})();