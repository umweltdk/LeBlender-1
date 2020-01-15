﻿angular.module("umbraco").controller("leblender.editormanager.delete",
	function ($scope, assetsService, $http, LeBlenderRequestHelper, dialogService, $routeParams, navigationService, treeService) {

		$scope.delete = function (id) {
			if ($scope.model.value && id) {
				$scope.editors.splice($scope.indexModel, 1);
				LeBlenderRequestHelper.deleteGridEditor($scope.model.value.id).then(function (response) {
					LeBlenderRequestHelper.getGridEditors().then(function (response) {
						$scope.editors = response;
						treeService.removeNode($scope.currentNode);
						navigationService.hideMenu();
					});
				});
			}
		};

		$scope.cancelDelete = function () {
			navigationService.hideNavigation();
		};

		LeBlenderRequestHelper.getGridEditors().then(function (response) {

			// init model
			$scope.editors = response

			// Init model value
			$scope.model = {
				value: {
					name: "",
					alias: "",
					view: "",
					icon: ""
				}
			};

			// look for the current editor
			_.each($scope.editors, function (editor, editorIndex) {
				if (editor.alias === $scope.currentNode.id) {
					$scope.indexModel = editorIndex;
					angular.extend($scope, {
						model: {
							value: editor
						}
					});
				}
			});

		});

	});
