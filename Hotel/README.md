# cephas-api-hotel

## Nuevos Endpoints: Reservations y Payments

A continuación, encontrarás los ejemplos de los payloads JSON que puedes utilizar en Postman para probar el CRUD de las entidades `Reservation` y `Payment`. Todos los endpoints operan bajo la ruta base de tu API (ejemplo: `https://localhost:7071/api/v1`).

---

### 🛏️ Reservations (`/api/v1/reservations`)

#### GET: Obtener Reservaciones Paginadas
- **URL**: `GET /api/v1/reservations/paged?page=1&pageSize=10&search=RES`
- **Nota**: El parámetro `search` buscará coincidencias en la columna `reservation_code`.

#### POST: Crear Reservación
- **URL**: `POST /api/v1/reservations`
- **Body (JSON)**:
```json
{
  "roomId": 1,
  "customerId": 1,
  "checkInDate": "2026-03-01T15:00:00Z",
  "checkOutDate": "2026-03-05T11:00:00Z",
  "reservationStatusId": 1,
  "paymentStatusId": 1,
  "totalAmount": 1500.00,
  "paidAmount": 500.00,
  "reservationCode": "RES001",
  "createdBy": "AdminUser"
}
```

#### PUT: Actualizar Reservación (Ej. ID: 1)
- **URL**: `PUT /api/v1/reservations/1`
- **Body (JSON)**:
```json
{
  "roomId": 1,
  "customerId": 1,
  "checkInDate": "2026-03-01T15:00:00Z",
  "checkOutDate": "2026-03-08T11:00:00Z",
  "reservationStatusId": 2,
  "paymentStatusId": 2,
  "totalAmount": 2000.00,
  "paidAmount": 2000.00,
  "reservationCode": "RES001",
  "createdBy": "AdminUser"
}
```

#### DELETE: Borrar Reservación Lógicamente (Ej. ID: 1)
- **URL**: `DELETE /api/v1/reservations/1`
- **Nota**: Esto actualizará `is_active` a `false`.

---

### 💳 Payments (`/api/v1/payments`)

#### GET: Obtener Pagos Paginados
- **URL**: `GET /api/v1/payments/paged?page=1&pageSize=10&search=TRX`
- **Nota**: El parámetro `search` buscará coincidencias en la columna `reference`.

#### POST: Crear Pago
- **URL**: `POST /api/v1/payments`
- **Body (JSON)**:
```json
{
  "reservationId": 1,
  "amount": 500.00,
  "paymentDate": "2026-03-01T14:30:00Z",
  "paymentMethod": "Credit Card",
  "reference": "TRX-123456789",
  "createdBy": "AdminUser"
}
```

#### PUT: Actualizar Pago (Ej. ID: 1)
- **URL**: `PUT /api/v1/payments/1`
- **Body (JSON)**:
```json
{
  "reservationId": 1,
  "amount": 500.00,
  "paymentDate": "2026-03-01T14:30:00Z",
  "paymentMethod": "Debit Card",
  "reference": "TRX-123456789",
  "createdBy": "AdminUser"
}
```

#### DELETE: Borrar Pago Lógicamente (Ej. ID: 1)
- **URL**: `DELETE /api/v1/payments/1`
- **Nota**: Esto actualizará `is_active` a `false`.
