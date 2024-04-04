# DnetQdrantAdmin
Datalnet Qdrant Vector Database Administrator 

# DnetQdrantAdmin

## Description
`DnetQdrantAdmin` is a web application designed for managing Qdrant vector database
## Key Features

### Collection Management
- **Creation and Management of Collections**: Allows users to create and manage data collections, including viewing detailed information about each collection.

### Data Point Operations
- **Creation and Deletion of Points**: Facilitates the addition and removal of data points within collections. 
Note. At the moment it is only possible to create Points by obtaining OpenAI embeddings. In future versions we will add other providers.

### Search and Similarity
- **Similarity-Based Search**: Offers the capability to perform advanced searches based on the similarity between data points.

### Llm Models and Providers Handling
- **Llm Models Integration**: Supports the configuration and use of specific models (OpenAI models).
- **Llm Providers Management**: Allows the administration of providers or sources of Llm models.

## Technologies Used
- .Net 8+
- Datalnet Blazor components

