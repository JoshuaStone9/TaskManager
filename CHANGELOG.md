# Changelog

All notable changes to the Coffee Shop Task Manager project will be documented in this file.

## [1.2.1] - 25/01/2026

### Added
- Environment variable support for sensitive configuration
- .env file for secure PIN storage (excluded from git)
- .gitignore file to exclude sensitive files and build artifacts from version control

### Changed
- ADMIN_PIN now retrieved from environment variables instead of hardcoded values
- Updated Program.cs to load and parse .env file on startup

### Technical Details
- Implemented .env file parsing in Program.cs for environment variable loading
- Modified Verify Pin method to read ADMIN_PIN from environment variables with fallback to "1234"

---

## [1.2.0] - 20/01/2026

### Added
- **Admin Dashboard with PIN Authentication**
  - Secure PIN-protected admin access (default PIN: 1234)
  - Session-based authentication with 30-minute timeout
  - Beautiful gradient lock screen UI with modern design
  - Admin dashboard accessible at `/Home/AdminDashboard`

- **Employee Management Features**
  - Add new employees through admin dashboard
  - Edit employee details and task assignments
  - View employee-specific task lists
  - Dedicated employee deletion section with warnings
  - Employee cards showing task counts and quick actions

- **Task Management Enhancements**
  - Create tasks with multi-employee assignment
  - Delete tasks with cascade removal of assignments
  - View all tasks with assignment status
  - Color-coded status indicators (Not Started, In Progress, Completed)
  - Real-time task status updates

- **Admin-Only Actions**
  - EditEmployee view for managing individual employee tasks
  - Assign/remove tasks from employees
  - Update employee names
  - Delete employees with automatic cleanup of task assignments

- **Security Features**
  - Session management for admin access
  - Protected admin routes with automatic PIN screen redirect
  - Authentication checks on all administrative actions

### Changed
- Refactored HomeController with admin authentication helper method
- Updated Program.cs to support session management
- Improved UI with gradient backgrounds and modern card designs
- Enhanced employee grid layout with responsive design
- Consolidated styles in site.css for better maintainability

### Fixed
- Task status color coding now properly reflects current state
- Database initialization on application startup
- Session configuration for persistent admin access
- Form validation and error handling improvements

### Technical Details
- Added `IsAdminAuthenticated()` helper method to HomeController
- Implemented distributed memory cache for session storage
- Created EnterPinScreen view with custom PIN input styling
- Added EditEmployee view for granular employee management
- Protected DELETE operations with confirmation dialogs

### UI/UX Improvements
- Modern PIN entry screen with lock icon and gradient button
- Color-coded task cards (green for completed, yellow for in-progress, gray for not started)
- Employee cards with gradient purple backgrounds
- Warning indicators for destructive actions
- Responsive grid layouts for tasks and employees
- Hover effects and smooth transitions throughout

---

## [1.1.0] - 22/01/2026

### Added
- Initial employee and task management system
- SQLite database with Entity Framework Core
- Basic MVC structure with Controllers and Views
- Task assignment system with many-to-many relationships

### Changed
- Database schema for TaskAssignment relationships

### Fixed
- Initial bug fixes and stability improvements

---

## [1.0.0] - 20/01/2026

### Added
- Project initialization
- Basic ASP.NET Core MVC setup
- SQLite database integration
- Employee and Task models
- Home controller and views
