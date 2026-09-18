param location string = 'swedencentral'
param storageAccountName string
param keyVaultName string
param logAnalyticsWorkspaceName string
param containerAppsEnvironmentName string
param backendName string
param containerRegistryServer string
param backendImage string
param applicationInsightsName string

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
module backend 'modules/backend-container-app.bicep' = {
  name: 'backend'
  params: {
    location: location
    backendName: backendName
    containerAppsEnvironmentId: containerAppsEnvironment.outputs.environmentId
    containerRegistryServer: containerRegistryServer
    backendImage: backendImage
    keyVaultUri: keyVault.outputs.keyVaultUri
    storageAccountName: storage.outputs.storageAccountName
    consultationImagesContainerName: storage.outputs.consultationImagesContainerName
    applicationInsightsConnectionString: applicationInsights.outputs.connectionString
  }
}
module keyVaultRbac 'modules/key-vault-rbac.bicep' = {
  name: 'keyVaultRbac'
  params: {
    keyVaultId: keyVault.outputs.keyVaultId
    principalId: backend.outputs.principalId
  }
}

module storageRbac 'modules/storage-rbac.bicep' = {
  name: 'storageRbac'
  params: {
    storageAccountName: storage.outputs.storageAccountName
    principalId: backend.outputs.principalId
  }
}
module applicationInsights 'modules/application-insights.bicep' = {
  name: 'applicationInsights'
  params: {
    location: location
    applicationInsightsName: applicationInsightsName
    logAnalyticsWorkspaceName: logAnalyticsWorkspaceName
  }
}

output deploymentLocation string = location