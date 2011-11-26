CREATE TABLE GRAPHS (graphID INT IDENTITY(1,1) CONSTRAINT GraphPKey PRIMARY KEY,
					 graphURI NVARCHAR(MAX),
					 graphHash INT NOT NULL);
					 
CREATE TABLE GRAPH_TRIPLES (graphID INT NOT NULL,
							tripleID INT NOT NULL,
							CONSTRAINT GraphTriplesPKey PRIMARY KEY (graphID, tripleID));

CREATE TABLE NODES (nodeID INT CONSTRAINT NodePKey PRIMARY KEY,
					nodeType TINYINT NOT NULL,
					nodeValue NVARCHAR(MAX) COLLATE Latin1_General_BIN,
					nodeHash INT NOT NULL);
					
CREATE TABLE TRIPLES (tripleID INT NOT NULL CONSTRAINT TriplePKey PRIMARY KEY,
					  tripleSubject INT NOT NULL,
					  triplePredicate INT NOT NULL,
					  tripleObject INT NOT NULL,
					  tripleHash INT NOT NULL);
					  
CREATE TABLE NS_URIS (nsUriID INT IDENTITY(1,1) CONSTRAINT NSUriPKey PRIMARY KEY,
					  nsUri NVARCHAR(MAX),
					  nsUriHash INT NOT NULL);
					  
CREATE TABLE NS_PREFIXES (nsPrefixID INT IDENTITY(1,1) CONSTRAINT NSPrefixPKey PRIMARY KEY,
						  nsPrefix NVARCHAR(50));
						  
CREATE TABLE NAMESPACES (nsID INT IDENTITY(1,1) CONSTRAINT NSPKey PRIMARY KEY,
						 graphID INT NOT NULL,
						 nsPrefixID INT NOT NULL,
						 nsUriID INT NOT NULL);