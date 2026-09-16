param location string
param containerAppsEnvironmentName string
param logAnalyticsWorkspaceName string

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2025-07-01' existing = {
  name: logAnalyticsWorkspaceName


}
resource containerAppsEnvironment 'Microsoft.App/managedEnvironments@2025-07-01' = {
  name: containerAppsEnvironmentName
  location: location

  identity: {
    type: 'SystemAssigned'
  }

  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalyticsWorkspace.properties.customerId
      }
    }

    publicNetworkAccess: 'Enabled'

    workloadProfiles: [
      {
        name: 'Consumption'
        workloadProfileType: 'Consumption'
      }
    ]

    zoneRedundant: false

    peerAuthentication: {
  mtls: {
    enabled: false
  }
}

peerTrafficConfiguration: {
  encryption: {
    enabled: false
  }
}
  }


}