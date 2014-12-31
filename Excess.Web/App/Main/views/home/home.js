﻿(function() {
    var controllerId = 'app.views.home';
    angular.module('app').controller(controllerId, [
        '$scope', '$modal', 'xsCompiler', function ($scope, $modal, xsCompiler) {
            var vm = this;
            
            $scope.translateSource = function () {
                getEditors();

                xsCompiler.translate(_sourceEditor.getValue())
                    .then(function (result) {
                        alert(result);
                    })
                    .catch(function () {
                        alert("Error");
                    });
            }

            $scope.newProject = function () {

                var modalInstance = $modal.open({
                    templateUrl: '/App/Main/dialogs/newProject.html',
                    controller: $scope.newProjectCtrl,
                    windowClass: "app-modal-window",
                    backdrop: true,
                    resolve: {
                    }
                });

                modalInstance.result.then(function(){
                }, function(){
                });
            }


            //code mirror must be resized manually
            $scope.resizeSource = function () {
                $scope.sourceResized = !$scope.sourceResized;
            }

            $scope.resizeTarget = function () {
                $scope.targetResized = !$scope.targetResized;
            }

            //code mirror has its own instances, which we need to cache
            var _sourceEditor, _targetEditor;

            function getEditors() {
                if (_sourceEditor && _targetEditor)
                    return;

                _sourceEditor = getCodeMirror('#source-editor');
                _targetEditor = getCodeMirror('#target-editor');
            }

            function getCodeMirror(elem)
            {
                var target = $(elem).isolateScope();
                if (!target || !target.instance)
                    throw "Element " + elem + " is not a code mirror directive";

                return target.instance;
            }
        }
    ]);
})();