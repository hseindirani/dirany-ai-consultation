param location string
param keyVaultName string

resource keyVault 'Microsoft.KeyVault/vaults@2025-05-01' = {
  name: keyVaultName
  location: location

  properties: {
    tenantId: tenant().tenantId
    sku: {
      family: 'A'
      name: 'standard'
    }

    enableRbacAuthorization: true
    enableSoftDelete: true
    softDeleteRetentionInDays: 90
    publicNetworkAccess: 'Enabled'

    networkAcls: {
      bypass: 'None'
      defaultAction: 'Allow'
    }
  }
}
output keyVaultId string = keyVault.id
output keyVaultUri string = keyVault.properties.vaultUri