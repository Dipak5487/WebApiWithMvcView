pipeline {
   agent any
   stages{

     stage ('Clean workspace') {
          steps {
           cleanWs()
          }
        }

      stage ('Git Checkout') {
  steps {
      git branch: 'main', credentialsId: 'demo-app-build', url: 'https://github.com/Dipak5487/WebApiWithMvcView.git'
    }
  }

      stage('Restore packages') {
  steps {
    bat "dotnet restore ${workspace}\\WebApiWithMvcView\\Crud.Demo.Web.Api.sln"
  }
}
   }
   
}
