# Service Contract Template
## [Service Name] API Contract

**Version:** 1.0  
**Owner:** [Team/Person]  
**Last Updated:** [Date]

## Service Overview
Brief description of what this service does and its business responsibility.

## API Endpoints

### GET /api/[resource]
**Purpose:** [What this endpoint does]  
**Authentication:** [Required | Optional | None]

**Request:**
```json
{
  "param1": "string",
  "param2": "number"
}
```

**Response:**
```json
{
  "data": [],
  "status": "success",
  "timestamp": "2025-01-01T00:00:00Z"
}
```

**Error Responses:**
- `400 Bad Request`: Invalid input parameters
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Service error

## Events Published
Events this service emits for other services to consume.

### [EventName]
**Trigger:** When [condition occurs]  
**Schema:**
```json
{
  "eventId": "uuid",
  "eventType": "EventName",
  "timestamp": "2025-01-01T00:00:00Z",
  "data": {
    "field1": "value",
    "field2": "value"
  }
}
```

## Events Consumed
Events this service listens to from other services.

### [EventName]
**Source Service:** [ServiceName]  
**Action:** What this service does when receiving this event

## Dependencies
- **Database:** [Database name/type]
- **External APIs:** [List of external dependencies]
- **Other Services:** [List of service dependencies]

## Data Ownership
- **Owns:** [List of data/entities this service is authoritative for]
- **Reads:** [List of data this service reads from others]

## SLA Requirements
- **Availability:** 99.9%
- **Response Time:** < 200ms for 95th percentile
- **Throughput:** [Expected requests per second]

## Deployment Information
- **Container:** [Docker image details]
- **Environment Variables:** [Required configuration]
- **Health Check:** [Health endpoint details]

---

*Keep this contract updated as the service evolves. Version changes appropriately.*