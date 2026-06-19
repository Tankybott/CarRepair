# CarRepair

A full-stack car repair shop management system built with ASP.NET Core 8 MVC. The application is split into two areas: a **customer portal** for browsing services and tracking repairs, and an **intranet panel** for staff to manage the full repair workflow.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 MVC |
| Language | C# / .NET 8 |
| Database | SQL Server + Entity Framework Core 8 |
| Identity & Auth | ASP.NET Core Identity (role-based: Admin, Manager, Employee, Customer) |
| Mapping | AutoMapper |
| Architecture | Repository pattern, Service layer, Dependency Injection |
| Frontend | Razor Views, Vanilla JS (fetch API / AJAX) |

---

## Project Structure

```
CarRepair/
├── CarRepair/               # MVC web host (Areas: Portal, Intranet)
├── Model/                   # Domain models, DTOs, ViewModels
├── DataAccess/              # EF Core DbContext, Migrations, Repositories
└── Service/                 # Business logic services
```

The project follows a clean layered architecture:

- **Repository layer** — generic `IRepository<T>` with async CRUD + soft delete pattern (`DeletedAt` timestamp)
- **Service layer** — all business logic lives here, fully interface-driven and unit tested
- **Controller layer** — thin controllers that delegate to services and return views or JSON

---

## Portal (Customer-Facing)

The public-facing area where customers can browse services, manage their cars, submit repair requests, and track repair progress.

### Home Page

The landing page content (title, description, opening hours schedule) is fully configurable from the intranet panel without touching any code.

> **Screenshot — Portal home page**
>
> *(insert screenshot here)*

---

### Service Offer & Cart

Customers can browse the service catalog organized by service type categories. Individual services can be added to a session-based cart. The cart persists across page navigation and shows a running list of selected services before checkout.

> **Screenshot — Offer page**
>
> *(insert screenshot here)*

> **Screenshot — Cart**
>
> *(insert screenshot here)*

---

### Car Management & Repair History

Authenticated customers can manage their registered vehicles (brand, model, year, VIN, engine code, fuel type, transmission, body type) and view all repairs per car. Each repair shows its current status in the workflow pipeline.

> **Screenshot — My cars & repairs view**
>
> *(insert screenshot here)*

---

### Submitting a Repair Request

From the cart, a logged-in customer selects one of their registered cars, adds an optional description, and submits. The repair is created in `Pending` status and handed off to the workshop.

Once the workshop produces a cost estimate and the manager reviews it, the customer can view the breakdown and **accept the quoted price** directly from their repair detail page, advancing the repair to the next stage.

---

## Intranet (Staff Panel)

Role-protected area for workshop staff. Access is controlled at action level:

| Role | Permissions |
|---|---|
| **Admin** | Full access — all management including employee create/edit/delete |
| **Manager** | Repair management, employee view, website config |
| **Employee** | Repair management (read + task operations) |

---

### Repair Management

The core of the application. Each repair moves through a defined status pipeline:

```
Pending → Manager Reviewed → Client Accepted → Scheduled → Done
                                                          ↘ Cancelled
```

The repair management panel covers:

- **Create** a repair for any registered client and car
- **Assign services** from the service catalog
- **Cost estimation** — line-item breakdown across three cost types: parts (unit price × quantity), labour (hourly rate), and overhead
- **Schedule** the repair (delivery date / pickup date) with automatic validation against the shop's weekly opening hours
- **Work tasks** — create, update, and cancel tasks within a repair, each with a time window and assigned employees
- **Overhead costs** — add and remove miscellaneous cost items

> **Screenshot — Repair management panel**
>
> *(insert screenshot here)*

---

### Employee Availability Checker

When creating or editing a work task, the system queries available employees in real time. Availability is filtered by:

- **Service type specialization** — only employees trained for the relevant service type are shown
- **Time window** — employees with an overlapping booking in the selected time slot are excluded

The endpoint returns a filtered list of employees that can be assigned to the task without conflicts.

> **Screenshot — Employee availability / task scheduling**
>
> *(insert screenshot here)*

---

### Employee Schedule View

Managers can inspect any employee's day timeline — showing all booked task slots against the shop's configured opening hours for that day of the week.

> **Screenshot — Employee day schedule**
>
> *(insert screenshot here)*

---

### Website Configuration

Admins and managers can edit the portal home page content and set the shop's weekly opening hours schedule (per day, with open/closed toggle) entirely from the intranet — no deployments needed.

---

### Other Management Modules

- **Clients** — create, update, soft-delete (cascades to cars and all related repairs)
- **Employees** — create with role and specializations, update including optional password reset, deactivate via account lockout
- **Cars** — full CRUD
- **Service Types** — CRUD; protected deletion (cannot delete a type that has active services)
- **Services** — CRUD; portal offer listing is driven by these
- **Parts** — CRUD with status tracking: `Ordered → Delivered → Installed`

