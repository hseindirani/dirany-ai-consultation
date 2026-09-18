param location string
param storageAccountName string

resource storageAccount 'Microsoft.Storage/storageAccounts@2025-06-01' = {
  name: storageAccountName
  location: location

  sku: {
    name: 'Standard_LRS'
  }

  kind: 'StorageV2'
}
resource blobService 'Microsoft.Storage/storageAccounts/blobServices@2025-06-01' = {
  parent: storageAccount
  name: 'default'

  properties: {
    deleteRetentionPolicy: {
      enabled: true
      days: 7
      allowPermanentDelete: false
    }

    containerDeleteRetentionPolicy: {
      enabled: true
      days: 7
    }
  }
}
resource consultationImagesContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2025-06-01' = {
  parent: blobService
  name: 'consultation-images'

  properties: {
    publicAccess: 'None'
  }
}
output storageAccountName string = storageAccount.name
output consultationImagesContainerName string = consultationImagesContainer.name