# Single Route Migration Command

## Overview
Execute migration of a single route following the CarTalk Consolidation Migration Protocol. This command implements the complete migration workflow with mandatory pre-analysis, AS-IS transfer, verification, and retrospective integration.

## Mission Briefing: Single Route Migration Protocol

You will now execute a single route migration using the **CarTalk Consolidation Migration Protocol.** This command follows the complete migration workflow with strict adherence to AS-IS transfer principles and mandatory retrospective integration.

---

## **Phase 0: Pre-Migration Analysis (Read-Only)**

**Directive:** Execute mandatory pre-migration analysis using `/analyze` command before touching ANY files.

### 0.1. Execute Deep Analysis
- **REQUIRED**: Run `/analyze` command on the source route file
- **REQUIRED**: Analyze the complete dependency chain
- **REQUIRED**: Map all imports and their relationships
- **REQUIRED**: Understand the page's complete architecture
- **REQUIRED**: Document all styling, UI, and UX patterns

### 0.2. Route Analysis
- **REQUIRED**: Read the source page file completely
- **REQUIRED**: Map all imports and their dependency chains
- **REQUIRED**: Understand the page's purpose and functionality
- **REQUIRED**: Document the page's data requirements
- **REQUIRED**: Identify any special routing patterns or dynamic segments

### 0.3. Component Mapping
- **REQUIRED**: List every component imported in the page
- **REQUIRED**: Read each component file to understand its dependencies
- **REQUIRED**: Map component prop interfaces and requirements
- **REQUIRED**: Identify any shared components vs page-specific components
- **REQUIRED**: Document component styling dependencies

### 0.4. Styling Analysis
- **REQUIRED**: Identify all CSS files imported or referenced
- **REQUIRED**: Map Chakra UI components and their versions (v1 vs v2)
- **REQUIRED**: Document any custom CSS modules or global styles
- **REQUIRED**: Identify theme dependencies and custom styling
- **REQUIRED**: Map responsive design patterns and breakpoints

### 0.5. Data Flow Analysis
- **REQUIRED**: Identify data fetching methods (getStaticProps, getServerSideProps, useEffect, etc.)
- **REQUIRED**: Map API calls and external service dependencies
- **REQUIRED**: Document state management patterns (Context, local state, etc.)
- **REQUIRED**: Identify any caching strategies or data transformations
- **REQUIRED**: Map GraphQL queries or REST API endpoints

### 0.6. API Integration
- **REQUIRED**: List all external API calls and their purposes
- **REQUIRED**: Document authentication requirements
- **REQUIRED**: Map error handling patterns
- **REQUIRED**: Identify any rate limiting or caching requirements
- **REQUIRED**: Document data transformation and validation logic

**Output:** Complete analysis report with all dependencies mapped.

**Constraint:** **No mutations are permitted during this phase.**

---

## **Phase 1: Migration Execution (AS-IS Transfer) or (1-to-1 Transfer). Changes to code structure are NOT PERMITTED**

**Directive:** Execute migration following the 7-step protocol exactly.

### STEP 1: File Transfer
- **REQUIRED**: Copy the exact file structure to the consolidated project
- **REQUIRED**: Maintain the same file name and extension initially
- **REQUIRED**: Preserve all original code comments and structure
- **REQUIRED**: Do not modify any logic during initial transfer

### STEP 2: JS→TSX Conversion
- **REQUIRED**: Change file extension from .js/.jsx to .ts/.tsx
- **REQUIRED**: Add proper TypeScript interfaces for all props
- **REQUIRED**: Add type annotations for all variables and functions
- **REQUIRED**: Ensure all imports have proper type definitions
- **REQUIRED**: Fix any TypeScript compilation errors without changing logic

### STEP 3: Import Resolution
- **REQUIRED**: Update all relative imports to use consolidated path structure
- **REQUIRED**: Ensure all components are available in the consolidated project
- **REQUIRED**: Update any path aliases (@/, ~/, etc.) to match consolidated config
- **REQUIRED**: Verify all imports resolve correctly
- **REQUIRED**: Do not change import functionality, only paths

### STEP 4: Style Transfer
- **REQUIRED**: Copy all CSS files referenced by the page
- **REQUIRED**: Update Chakra UI v1 components to v2 if needed
- **REQUIRED**: Ensure all CSS modules are properly imported
- **REQUIRED**: Verify theme configurations are compatible
- **REQUIRED**: Test that visual appearance matches the original exactly

### STEP 5: Type Safety
- **REQUIRED**: Create TypeScript interfaces for all data structures
- **REQUIRED**: Add proper typing for all function parameters and return values
- **REQUIRED**: Ensure all component props are properly typed
- **REQUIRED**: Add type guards for any dynamic data
- **REQUIRED**: Ensure no 'any' types are used unless absolutely necessary

### STEP 6: Dependency Resolution
- **REQUIRED**: Verify all imported components exist in consolidated project
- **REQUIRED**: Ensure all external dependencies are installed
- **REQUIRED**: Check that all API endpoints are accessible
- **REQUIRED**: Verify all utility functions are available
- **REQUIRED**: Test that all imports resolve without errors

### STEP 7: Complete Dependency Migration
- **REQUIRED**: Migrate ALL imported components to consolidated project
- **REQUIRED**: Convert ALL imported JavaScript files to TypeScript
- **REQUIRED**: Update ALL Chakra UI v1 components to v2 in imported files
- **REQUIRED**: Ensure ALL CSS and styling dependencies are migrated
- **REQUIRED**: Verify ALL utility functions and helpers are available
- **REQUIRED**: Complete ALL dependency chains before proceeding

---

## **Phase 2: Migration Verification**

**Directive:** Complete ALL verification steps before marking as complete.

### 1. Functionality Testing
- **REQUIRED**: Test all user interactions and form submissions
- **REQUIRED**: Verify all navigation and routing works
- **REQUIRED**: Test all data fetching and API calls
- **REQUIRED**: Verify all dynamic content renders correctly
- **REQUIRED**: Test all error handling and edge cases

### 2. Style Verification
- **REQUIRED**: Compare visual appearance with original page
- **REQUIRED**: Test responsive design on multiple screen sizes
- **REQUIRED**: Verify all animations and transitions work
- **REQUIRED**: Check that all images and icons display correctly
- **REQUIRED**: Ensure color schemes and typography match

### 3. Type Checking
- **REQUIRED**: Run TypeScript compiler and fix all errors
- **REQUIRED**: Ensure no TypeScript warnings remain
- **REQUIRED**: Verify all type definitions are correct
- **REQUIRED**: Test that type checking catches real errors
- **REQUIRED**: Ensure strict TypeScript compliance

### 4. Build Testing
- **REQUIRED**: Run Next.js build process and verify success
- **REQUIRED**: Check that all static generation works correctly
- **REQUIRED**: Verify no build warnings or errors
- **REQUIRED**: Test that the page loads in production mode
- **REQUIRED**: Ensure all assets are properly bundled

### 5. Integration Testing
- **REQUIRED**: Test navigation to and from the migrated page
- **REQUIRED**: Verify shared components work correctly
- **REQUIRED**: Test that data flows correctly between pages
- **REQUIRED**: Ensure no conflicts with other migrated code
- **REQUIRED**: Test that the page integrates with the overall app

---

## **Phase 3: Retrospective Integration**

**Directive:** Execute this workflow after EVERY single migration.

### 1. Execute `/retro` Command
- **REQUIRED**: Run the `/retro` command immediately after migration
- **REQUIRED**: Analyze what went right and what went wrong
- **REQUIRED**: Document any issues encountered during migration
- **REQUIRED**: Identify patterns that could improve future migrations
- **REQUIRED**: Capture any technical insights or best practices

### 2. Document Migration Results
- **REQUIRED**: List all problems encountered and their root causes
- **REQUIRED**: Document successful patterns that should be repeated
- **REQUIRED**: Identify any process improvements needed
- **REQUIRED**: Note any technical debt or shortcuts taken
- **REQUIRED**: Document any performance or quality issues

### 3. Re-analyze Project State
- **REQUIRED**: Execute `/analyze` command to understand current project state
- **REQUIRED**: Verify all dependencies are still intact
- **REQUIRED**: Check that no regressions were introduced
- **REQUIRED**: Understand the current project architecture
- **REQUIRED**: Identify any new patterns or relationships

### 4. Update Documentation
- **REQUIRED**: Mark the completed migration as done in consolidation-tasks.md
- **REQUIRED**: Update progress percentages and metrics
- **REQUIRED**: Document any new learnings in the notes section
- **REQUIRED**: Update quality metrics based on migration results
- **REQUIRED**: Prepare for the next migration with updated knowledge

---

## **Phase 4: Final Report & Verdict**

**Report Structure:**
- **Route Migrated**: Source and destination paths
- **Changes Applied**: List of all created/modified artifacts
- **Verification Evidence**: Commands and outputs proving migration success
- **Quality Metrics**: TypeScript coverage, style accuracy, functionality preservation
- **Retrospective Results**: Key learnings and process improvements
- **Final Verdict**: 
  - `"Migration Complete. Route successfully migrated with 100% functionality preservation. Mission accomplished."`
  - OR `"Migration Failed. CRITICAL ISSUE FOUND. [Describe issue and recommend steps]."`

**Progress Tracking:** Maintain inline TODO ledger using ✅ / ⚠️ / 🚧 markers throughout the process.

## Usage Examples
- `/migrate-route car-reviews/src/pages/index.tsx`
- `/migrate-route cartalk_frontend/pages/auto-loans/index.js`
- `/migrate-route car-reviews/src/pages/[make]/index.tsx`
- `/migrate-route cartalk_frontend/pages/drivers-ed/ca-provider-listing.js`

## Critical Restrictions

**FORBIDDEN ACTIONS:**
- **FORBIDDEN**: Modifying business logic, functionality, or behavior
- **FORBIDDEN**: Changing visual appearance beyond v1→v2 upgrades
- **FORBIDDEN**: Starting migration without complete pre-analysis
- **FORBIDDEN**: Skipping `/retro` command after migration
- **FORBIDDEN**: Not updating progress documentation
- **FORBIDDEN**: Using 'any' types in TypeScript unless absolutely necessary
- **FORBIDDEN**: Leaving TypeScript compilation errors
- **FORBIDDEN**: Breaking existing functionality
- **FORBIDDEN**: Building the project before ALL dependencies are migrated

**REQUIRED ACTIONS:**
- **REQUIRED**: Execute `/analyze` command before migration
- **REQUIRED**: Complete pre-migration analysis before any changes
- **REQUIRED**: AS-IS transfer with JS→TSX conversion only
- **REQUIRED**: Execute `/retro` command after migration
- **REQUIRED**: Update consolidation-tasks.md with progress
- **REQUIRED**: Maintain 100% functionality preservation
- **REQUIRED**: Ensure visual appearance matches original exactly