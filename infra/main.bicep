param location string = 'swedencentral'
param storageAccountName string
param keyVaultName string
param logAnalyticsWorkspaceName string
param containerAppsEnvironmentName string

module storage 'modules/storage.bicep' = {
  name: 'storage'
  params: {
    location: location
    storageAccountName: storageAccountName
  }
}

module keyVault 'modules/key-vault.bicep' = {
  name: 'keyVault'
  params: {
    location: location
    keyVaultName: keyVaultName
  }
}

module containerAppsEnvironment 'modules/container-app-environment.bicep' = {
  name: 'containerAppsEnvironment'
  params: {
    location: location
    containerAppsEnvironmentName: containerAppsEnvironmentName
    logAnalyticsWorkspaceName: logAnalyticsWorkspaceName
  }
}

output deploymentLocation string = location