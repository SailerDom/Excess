
var xsServices = angular.module('xs.Services', []);

xsServices.service('excess_ui.Project', ['$http', '$q', function($http, $q)
{
	
this.compile = function (code)
{
	var deferred = $q.defer();

    $http.post("/project/compile", {
		code : code,

	}).then(function(__response) {
		deferred.resolve(__response.data.__res);
	}, function(ex){
		deferred.reject(ex);
    });

    return deferred.promise;
}



this.load = function (projectName)
{
	var deferred = $q.defer();

    $http.post("/project/load", {
		projectName : projectName,

	}).then(function(__response) {
		deferred.resolve(__response.data.__res);
	}, function(ex){
		deferred.reject(ex);
    });

    return deferred.promise;
}



this.readFile = function (fileID)
{
	var deferred = $q.defer();

    $http.post("/project/readFile", {
		fileID : fileID,

	}).then(function(__response) {
		deferred.resolve(__response.data.__res);
	}, function(ex){
		deferred.reject(ex);
    });

    return deferred.promise;
}



this.saveFile = function (fileID,content)
{
	var deferred = $q.defer();

    $http.post("/project/saveFile", {
		fileID : fileID,
content : content,

	}).then(function(__response) {
		deferred.resolve(__response.data.__res);
	}, function(ex){
		deferred.reject(ex);
    });

    return deferred.promise;
}



this.deleteFile = function (fileID)
{
	var deferred = $q.defer();

    $http.post("/project/deleteFile", {
		fileID : fileID,

	}).then(function(__response) {
		deferred.resolve(__response.data.__res);
	}, function(ex){
		deferred.reject(ex);
    });

    return deferred.promise;
}



this.newFile = function (parentFileID,fileName)
{
	var deferred = $q.defer();

    $http.post("/project/newFile", {
		parentFileID : parentFileID,
fileName : fileName,

	}).then(function(__response) {
		deferred.resolve(__response.data.__res);
	}, function(ex){
		deferred.reject(ex);
    });

    return deferred.promise;
}



    this.__ID = '6f5ea846-8803-47d0-8929-2629b41cf791';
}])


