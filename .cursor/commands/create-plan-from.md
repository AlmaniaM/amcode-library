# Create Plan From Component Command

## Purpose
Comprehensively analyze a target component and all its children in extreme detail to extract complete implementation specifications. Creates detailed recreation plans that enable another LLM to recreate the component entirely without access to the original codebase. Generates separate plan files per component to maintain organization and avoid context overload.

## When to Use
- **Component Recreation**: When you need to recreate a component in a different codebase or framework
- **Component Documentation**: Creating comprehensive documentation for complex component systems
- **Migration Planning**: Understanding component structure before migration to a new framework
- **Code Handoff**: Providing complete component specifications for new team members or LLMs
- **Pattern Extraction**: Extracting design patterns, styling approaches, and architectural decisions
- **Feature Analysis**: Deep analysis of component features and functionality
- **Multi-LLM Coordination**: Creating detailed plans that other AI agents can execute independently

## Command Structure

### Phase 1: Target Component Discovery & Initial Analysis
**AUTONOMOUS PRINCIPAL ENGINEER DOCTRINE**: Begin with comprehensive reconnaissance to understand the target component and its context within the project ecosystem.

#### Component Identification
- **Target Component Path**: Identify the exact file path of the target component
- **Component Type**: Determine component type (React, Vue, Angular, Svelte, etc.)
- **Component Role**: Understand the component's purpose and responsibility
- **Entry Point Analysis**: Identify how the component is imported and used
- **Export Analysis**: Understand what the component exports (default, named, types, etc.)

#### Project Context Gathering
- **Technology Stack Detection**: Identify framework, language, build tools, and dependencies
- **Project Structure**: Understand project organization and component location patterns
- **Styling System**: Identify styling approach (CSS modules, styled-components, Tailwind, etc.)
- **Theme System**: Understand theme configuration and theming patterns
- **State Management**: Identify state management patterns and libraries
- **Routing Context**: Understand routing patterns if component is route-related

#### Initial Component Analysis
- **Component Signature**: Extract component props, state, and interface definitions
- **Dependencies**: Identify all imports and external dependencies
- **File Structure**: Understand component file organization (single file, multi-file, etc.)
- **Complexity Assessment**: Assess component complexity and child component count
- **Pattern Recognition**: Identify initial patterns (HOC, render props, hooks, etc.)

### Phase 2: Recursive Child Component Discovery
**Technology-Adaptive Protocols**: Systematically discover and catalog all child components recursively.

#### Direct Child Component Discovery
- **Import Analysis**: Extract all component imports from the target file
- **Component Usage**: Identify all components used within the target component
- **Dynamic Imports**: Identify lazy-loaded or dynamically imported components
- **Conditional Rendering**: Map components rendered conditionally
- **List Rendering**: Identify components rendered in loops or lists

#### Recursive Traversal
- **Child Component Files**: Locate and read all child component files
- **Nested Component Discovery**: Recursively discover components within child components
- **Component Tree Mapping**: Build complete component hierarchy tree
- **Circular Dependency Detection**: Identify and handle circular dependencies
- **External Component Identification**: Distinguish between project components and external library components

#### Component Relationship Mapping
- **Parent-Child Relationships**: Map parent-child component relationships
- **Sibling Relationships**: Identify sibling components and their interactions
- **Data Flow**: Map data flow between components (props, context, state)
- **Event Flow**: Map event handling and communication patterns
- **Dependency Graph**: Create complete dependency graph of all components

### Phase 3: Deep Component Analysis
**Universal Frontend Stewardship**: Extract comprehensive implementation details from each component.

#### Component Structure Analysis
- **Component Definition**: Extract component type (functional, class, HOC, etc.)
- **Props Interface**: Extract complete props interface with types and defaults
- **State Management**: Extract all state variables, their types, and initialization
- **Lifecycle Hooks**: Extract all lifecycle hooks and their implementations
- **Custom Hooks**: Extract custom hooks used and their implementations
- **Refs and DOM Access**: Extract ref usage and DOM manipulation patterns

#### Feature Extraction
- **Core Features**: Extract all core functionality and features
- **User Interactions**: Extract all user interaction handlers (click, input, scroll, etc.)
- **Data Operations**: Extract data fetching, transformation, and manipulation
- **Validation Logic**: Extract validation rules and error handling
- **Business Logic**: Extract business rules and calculations
- **Side Effects**: Extract all side effects (API calls, subscriptions, timers, etc.)

#### Styling & Theme Extraction
- **Style Definitions**: Extract all StyleSheet definitions and CSS
- **Theme Usage**: Extract theme color, typography, spacing, and shape usage
- **Responsive Design**: Extract responsive breakpoints and adaptive styles
- **Animation Patterns**: Extract animation definitions and timing
- **Layout Patterns**: Extract layout patterns (flexbox, grid, absolute positioning)
- **Visual States**: Extract styling for different states (hover, active, disabled, etc.)
- **Platform-Specific Styles**: Extract platform-specific styling (iOS, Android, Web)

#### Pattern Extraction
- **Design Patterns**: Identify design patterns (Observer, Factory, Strategy, etc.)
- **Component Patterns**: Identify component patterns (Container/Presentational, Compound, etc.)
- **State Patterns**: Identify state management patterns (lifting state, context, reducers)
- **Performance Patterns**: Identify performance optimizations (memoization, virtualization, etc.)
- **Accessibility Patterns**: Identify accessibility patterns (ARIA, semantic HTML, etc.)
- **Error Handling Patterns**: Identify error handling and boundary patterns

#### Data Flow Analysis
- **Props Flow**: Map how props flow through component hierarchy
- **State Flow**: Map state updates and propagation
- **Context Usage**: Extract context providers and consumers
- **Event Propagation**: Map event handling and propagation
- **Data Transformation**: Extract data transformation and mapping logic
- **API Integration**: Extract API calls and data fetching patterns

### Phase 4: Supporting Code & Dependency Analysis
**Comprehensive Dependency Understanding**: Analyze all supporting code, utilities, and dependencies.

#### Utility & Helper Analysis
- **Utility Functions**: Extract all utility functions used by components
- **Helper Functions**: Extract helper functions and their implementations
- **Constants**: Extract constants, enums, and configuration values
- **Type Definitions**: Extract TypeScript/Flow type definitions and interfaces
- **Validation Schemas**: Extract validation schemas and rules

#### External Dependency Analysis
- **Library Dependencies**: Extract all external library dependencies
- **Library Usage Patterns**: Extract how external libraries are used
- **Version Information**: Extract dependency versions from package files
- **Configuration**: Extract library configuration and setup
- **Custom Wrappers**: Extract custom wrappers around external libraries

#### Service & API Analysis
- **API Services**: Extract API service definitions and usage
- **Data Models**: Extract data models and their structure
- **API Endpoints**: Extract API endpoints and request/response patterns
- **Error Handling**: Extract API error handling patterns
- **Caching Strategies**: Extract caching and data persistence patterns

#### Asset Analysis
- **Images**: Extract image assets and their usage
- **Icons**: Extract icon usage and icon library patterns
- **Fonts**: Extract font definitions and typography assets
- **Static Assets**: Extract static asset usage patterns

### Phase 5: Plan Generation & Organization
**Multi-Layer Verification**: Generate comprehensive, organized plans for each component.

#### Plan Structure Design
- **Component Plan Template**: Design standardized plan template for each component
- **Plan Organization**: Organize plans by component hierarchy
- **Cross-References**: Create cross-references between related components
- **Dependency Mapping**: Include dependency information in each plan
- **Implementation Order**: Suggest implementation order based on dependencies

#### Component Plan Contents
Each component plan MUST include:

##### Component Overview
- **Component Name**: Full component name and file path
- **Component Purpose**: Clear description of component purpose and responsibility
- **Component Type**: Component type (functional, class, HOC, etc.)
- **Dependencies**: List of all dependencies (components, hooks, utilities, libraries)
- **Usage Context**: How and where the component is used

##### Complete Implementation Specifications
- **Props Interface**: Complete props interface with types, defaults, and descriptions
- **State Management**: All state variables with types, initialization, and update logic
- **Component Structure**: Complete component JSX/TSX structure
- **Event Handlers**: All event handlers with complete implementations
- **Lifecycle Logic**: All lifecycle hooks and their implementations
- **Custom Hooks**: All custom hooks with complete implementations
- **Business Logic**: All business logic and calculations
- **Data Operations**: All data fetching, transformation, and manipulation

##### Styling Specifications
- **Style Definitions**: Complete StyleSheet or CSS definitions
- **Theme Integration**: Complete theme usage (colors, typography, spacing, shapes)
- **Responsive Styles**: All responsive breakpoints and adaptive styles
- **Animation Definitions**: All animations with timing and easing
- **Layout Specifications**: Complete layout specifications (flexbox, grid, positioning)
- **Visual States**: Styling for all states (idle, hover, active, disabled, focus, etc.)
- **Platform-Specific Styles**: All platform-specific styling requirements

##### Pattern Documentation
- **Design Patterns**: All design patterns used and their implementations
- **Component Patterns**: Component patterns and their purposes
- **State Patterns**: State management patterns and rationale
- **Performance Patterns**: Performance optimizations and their purposes
- **Accessibility Patterns**: Accessibility implementations and requirements
- **Error Handling**: Error handling patterns and error boundaries

##### Data Flow Documentation
- **Props Flow**: Complete props flow through component hierarchy
- **State Flow**: State updates and propagation patterns
- **Event Flow**: Event handling and propagation patterns
- **Context Usage**: Context providers and consumers
- **API Integration**: API calls and data fetching patterns

##### Supporting Code
- **Utility Functions**: All utility functions with implementations
- **Helper Functions**: All helper functions with implementations
- **Constants**: All constants, enums, and configuration
- **Type Definitions**: All type definitions and interfaces
- **Validation Logic**: All validation schemas and rules

##### Dependencies & Requirements
- **External Libraries**: All external libraries with versions and usage
- **Internal Dependencies**: All internal component and utility dependencies
- **Asset Requirements**: All required assets (images, icons, fonts)
- **Configuration Requirements**: All required configuration and setup

##### Implementation Checklist
- **Prerequisites**: All prerequisites before implementation
- **Implementation Steps**: Step-by-step implementation guide
- **Testing Requirements**: Testing requirements and test cases
- **Validation Criteria**: Success criteria and validation requirements

#### Plan File Organization
- **File Naming Convention**: Use component name for plan file name
- **Directory Structure**: Organize plans in target folder maintaining component hierarchy
- **Index File**: Create index file mapping components to plan files
- **Summary File**: Create summary file with component overview and relationships
- **Dependency Graph**: Create visual or text-based dependency graph

### Phase 6: Plan Validation & Quality Assurance
**Comprehensive Quality Validation**: Ensure plans are complete, accurate, and actionable.

#### Completeness Validation
- **Component Coverage**: Verify all components are analyzed and documented
- **Feature Completeness**: Verify all features are extracted and documented
- **Styling Completeness**: Verify all styling is extracted and documented
- **Pattern Completeness**: Verify all patterns are identified and documented
- **Dependency Completeness**: Verify all dependencies are identified and documented

#### Accuracy Validation
- **Code Accuracy**: Verify extracted code matches original implementation
- **Type Accuracy**: Verify type definitions are accurate and complete
- **Style Accuracy**: Verify styling matches original implementation
- **Logic Accuracy**: Verify business logic is accurately represented
- **Pattern Accuracy**: Verify pattern identification is accurate

#### Actionability Validation
- **Implementation Clarity**: Verify plans are clear enough for implementation
- **Step Completeness**: Verify implementation steps are complete
- **Dependency Clarity**: Verify dependencies are clearly documented
- **Example Completeness**: Verify examples and code snippets are complete
- **Context Sufficiency**: Verify sufficient context is provided

#### Quality Gates
- **Completeness Gate**: All components must have complete plans
- **Accuracy Gate**: All extracted information must be accurate
- **Clarity Gate**: All plans must be clear and actionable
- **Organization Gate**: All plans must be well-organized
- **Cross-Reference Gate**: All cross-references must be valid

### Phase 7: Plan Output & Documentation
**Comprehensive Documentation**: Generate final plan files and supporting documentation.

#### Plan File Generation
- **Individual Component Plans**: Generate plan file for each component
- **Plan Format**: Use Markdown format for readability and LLM consumption
- **Code Examples**: Include complete code examples in plans
- **Visual Aids**: Include diagrams or visual representations when helpful
- **Cross-References**: Include cross-references to related components

#### Supporting Documentation
- **Index File**: Generate index file mapping all components
- **Summary File**: Generate summary with component overview
- **Dependency Graph**: Generate dependency graph visualization
- **Implementation Guide**: Generate overall implementation guide
- **Quick Reference**: Generate quick reference for common tasks

#### Plan Metadata
- **Generation Date**: Include plan generation date and time
- **Source Information**: Include source component paths and versions
- **LLM Context**: Include context for LLM consumption
- **Validation Status**: Include validation status and quality metrics
- **Usage Instructions**: Include instructions for using the plans

## Universal Component Analysis Protocols

### Technology-Adaptive Analysis
- **Framework-Specific Patterns**: Adapt analysis to specific framework patterns
- **Language-Specific Conventions**: Apply language-specific best practices
- **Build Tool Integration**: Understand build tool configurations and optimizations
- **Testing Framework**: Understand testing patterns and requirements

### Complexity Handling
- **Large Components**: Break down large components into manageable sections
- **Deep Hierarchies**: Handle deep component hierarchies systematically
- **Circular Dependencies**: Identify and document circular dependencies
- **Dynamic Components**: Handle dynamically generated or loaded components

### Quality Assurance
- **Completeness Verification**: Ensure all aspects are covered
- **Accuracy Validation**: Verify extracted information is accurate
- **Consistency Check**: Ensure information is internally consistent
- **Gap Identification**: Identify areas needing deeper investigation

## Usage Examples

### Basic Component Analysis
```
/create-plan-from
Analyze the RecipesListScreen component and all its children. Save plans to planning/component-plans/recipes-list-screen/
```

### Specific Target Folder
```
/create-plan-from
Extract complete implementation plan for RecipeCard component and children. Save to planning/recreation-plans/recipe-card/
```

### With Understanding Commands
```
/create-plan-from
Use /understand to analyze RecipesListScreenDesktop component first, then create detailed recreation plans. Save to planning/component-specs/
```

### Multiple Components
```
/create-plan-from
Analyze all components in src/components/recipes/RecipesListScreen/ directory. Create separate plans for each. Save to planning/recipes-list-screen-plans/
```

## Success Criteria

### Analysis Completeness
- **Component Coverage**: 100% of components analyzed and documented
- **Feature Extraction**: 100% of features extracted and documented
- **Styling Extraction**: 100% of styling extracted and documented
- **Pattern Identification**: 100% of patterns identified and documented
- **Dependency Mapping**: 100% of dependencies identified and mapped

### Plan Quality
- **Completeness**: All plans are complete and comprehensive
- **Accuracy**: All extracted information is accurate
- **Clarity**: All plans are clear and actionable
- **Organization**: All plans are well-organized
- **Cross-References**: All cross-references are valid

### Implementation Readiness
- **Actionable Plans**: Plans enable complete component recreation
- **Clear Steps**: Implementation steps are clear and complete
- **Sufficient Context**: Sufficient context provided for implementation
- **Dependency Clarity**: All dependencies are clearly documented
- **Validation Criteria**: Success criteria are clearly defined

## Integration with Other Commands

### Understanding Commands
- **Use `/understand`**: For deep understanding of complex components before analysis
- **Use `/understand-project`**: For project context and technology stack understanding
- **Use `/analyze-code-deep`**: For comprehensive code analysis before plan creation

### Implementation Commands
- **Use with `/do`**: Plans can be used to implement components in new codebases
- **Use with `/feature-implement`**: Plans provide specifications for feature implementation
- **Use with `/migrate`**: Plans help understand components before migration

### Analysis Commands
- **Use with `/analyze-component-architecture`**: For architectural analysis before plan creation
- **Use with `/analyze-performance`**: For performance analysis before plan creation
- **Use with `/refactor`**: Plans help understand components before refactoring

## Plan File Structure

### Individual Component Plan Structure
```markdown
# [Component Name] Implementation Plan

## Component Overview
- **Component Name**: [Full component name]
- **File Path**: [Original file path]
- **Component Type**: [Functional/Class/HOC/etc.]
- **Purpose**: [Component purpose and responsibility]
- **Dependencies**: [List of dependencies]

## Props Interface
[Complete props interface with types, defaults, descriptions]

## State Management
[All state variables with types, initialization, update logic]

## Component Structure
[Complete component JSX/TSX structure]

## Event Handlers
[All event handlers with complete implementations]

## Lifecycle & Hooks
[All lifecycle hooks and custom hooks]

## Business Logic
[All business logic and calculations]

## Styling Specifications
[Complete styling with theme integration]

## Patterns Used
[All design patterns and their implementations]

## Data Flow
[Props flow, state flow, event flow]

## Supporting Code
[Utility functions, helpers, constants, types]

## Dependencies & Requirements
[External libraries, internal dependencies, assets]

## Implementation Checklist
[Step-by-step implementation guide]
```

### Index File Structure
```markdown
# Component Plans Index

## Overview
[Overview of all analyzed components]

## Component List
- [Component Name] - [Plan File Path]
- [Component Name] - [Plan File Path]

## Dependency Graph
[Visual or text-based dependency graph]

## Implementation Order
[Suggested implementation order based on dependencies]
```

## Quality Assurance Checklist

### Analysis Quality
- [ ] All components discovered and analyzed
- [ ] All features extracted and documented
- [ ] All styling extracted and documented
- [ ] All patterns identified and documented
- [ ] All dependencies identified and mapped

### Plan Quality
- [ ] All plans are complete and comprehensive
- [ ] All extracted information is accurate
- [ ] All plans are clear and actionable
- [ ] All plans are well-organized
- [ ] All cross-references are valid

### Implementation Readiness
- [ ] Plans enable complete component recreation
- [ ] Implementation steps are clear and complete
- [ ] Sufficient context provided
- [ ] All dependencies clearly documented
- [ ] Success criteria clearly defined

**Remember**: The `/create-plan-from` command is your comprehensive component analysis and plan generation tool. It extracts every detail needed to recreate components entirely, creating detailed, actionable plans that enable complete component recreation without access to the original codebase. Use it whenever you need to document, migrate, or recreate complex component systems.

