// resource storageAccount 'Microsoft.Storage/storageAccounts@2021-08-01' = {
//   name: 'ComputerStoreAPI'
//   location: 'eastus'
//   sku: {
//     name: 'Standard_LRS'
//   }
//   kind: 'StorageV2'
//   properties: {
//     accessTier: 'Hot'
//   }
// }

// resource appServicePlan 'Microsoft.Web/serverfarms@2021-03-01' = {
//   name: 'computer-store-launch-plan-starter'
//   location: 'eastus'
//   sku: {
//     name: 'F1'
//   }
// }

// resource appServiceApp 'Microsoft.Web/sites@2021-03-01' = {
//   name: 'computer-store-launch-1'
//   location: 'eastus'
//   properties: {
//     serverFarmId: appServicePlan.id
//     httpsOnly: true
//   }
// }
