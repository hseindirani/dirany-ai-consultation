param location string
param backendName string
param containerAppsEnvironmentId string
param containerRegistryServer string
param backendImage string
param keyVaultUri string
param storageAccountName string
param consultationImagesContainerName string
param applicationInsightsConnectionString string

var dbConnectionSecretUri = '${keyVaultUri}secrets/db-connection'
var azureAiKeySecretUri = '${keyVaultUri}secrets/azure-ai-key'

resource backend 'Microsoft.App/containerApps@2025-02-02-preview' = {
  name: backendName
  location: location

  identity: {
    type: 'SystemAssigned'
  }

  properties: {
    managedEnvironmentId: containerAppsEnvironmentId

    configuration: {
      activeRevisionsMode: 'Single'

      ingress: {
        external: true
        targetPort: 8080
        transport: 'Auto'
        allowInsecure: false
        traffic: [
  {
    latestRevision: true
    weight: 100
  }
]
      }
      registries: [
       {
    server: containerRegistryServer
    identity: 'system-environment'
        }
              ]
        secrets: [
        {
            name: 'db-connection'
            keyVaultUrl: dbConnectionSecretUri
            identity: 'system'
  }
  {
              name: 'azure-ai-key'
              keyVaultUrl: azureAiKeySecretUri
              identity: 'system'
  }
]
    }
    template: {
        containers: [
        {
          name: backendName
          image: backendImage

         resources: {
           cpu: json('0.5')
         memory: '1Gi'
      }
        env: [
  {
    name: 'ConnectionStrings__DefaultConnection'
    secretRef: 'db-connection'
  }
  {
    name: 'AzureOpenAI__ApiKey'
    secretRef: 'azure-ai-key'
  } 
   {
  name: 'Storage__Provider'
  value: 'Azure'
}
{
  name: 'AzureStorage__AccountName'
  value: storageAccountName
}
{
  name: 'AzureStorage__ContainerName'
  value: consultationImagesContainerName
}
{
  name: 'AzureOpenAI__Endpoint'
  value: 'https://diranyai-foundry-dev.services.ai.azure.com/openai/v1'
}
{
  name: 'AzureOpenAI__DeploymentName'
  value: 'gpt-image-2'
}
{
  name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
  value: applicationInsightsConnectionString
}
]
    }
  ]
        scale: {
  minReplicas: 0
  maxReplicas: 10
  cooldownPeriod: 300
  pollingInterval: 30

  rules: [
    {
      name: 'http-scaler'
      http: {
        metadata: {
          concurrentRequests: '10'
        }
      }
    }
  ]
}
}
workloadProfileName: 'Consumption'

  }
}
output principalId string = backend.identity.principalId