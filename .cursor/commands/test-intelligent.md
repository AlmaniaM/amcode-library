# Test Command

## Purpose
Execute comprehensive testing strategies using automated testing, manual testing, and quality assurance approaches that align with Cursor's recommended testing methodologies for large-scale implementations.

## When to Use
- **Feature Testing**: When testing new features or functionality
- **Regression Testing**: When testing to ensure changes don't break existing functionality
- **Integration Testing**: When testing system integration and API interactions
- **Performance Testing**: When testing system performance and scalability
- **Security Testing**: When testing security vulnerabilities and compliance
- **User Acceptance Testing**: When validating user experience and requirements

## Command Structure

### Phase 1: Test Strategy & Planning
**AUTONOMOUS PRINCIPAL ENGINEER DOCTRINE**: Begin with comprehensive test planning and strategy definition.

#### Test Strategy Definition
- **Testing Objectives**: Define what needs to be tested and why
- **Testing Scope**: Determine what's in scope and out of scope for testing
- **Testing Approach**: Choose appropriate testing methodologies and tools
- **Quality Criteria**: Define success criteria and quality gates
- **Risk Assessment**: Identify testing risks and mitigation strategies

#### Test Planning
- **Test Case Design**: Create comprehensive test cases for all scenarios
- **Test Data Preparation**: Prepare test data and test environments
- **Test Environment Setup**: Configure testing environments and tools
- **Test Schedule**: Plan testing timeline and resource allocation
- **Test Documentation**: Create testing documentation and procedures

### Phase 2: Automated Testing Implementation
**Edit-Test-Fix Loop**: Implement Cursor's recommended automated testing approach.

#### Unit Testing
- **Component Testing**: Test individual components and functions
- **Service Testing**: Test services and business logic
- **Utility Testing**: Test utility functions and helpers
- **Model Testing**: Test data models and validation logic
- **Integration Testing**: Test component interactions and dependencies

#### Integration Testing
- **API Testing**: Test API endpoints and responses
- **Database Testing**: Test database operations and data integrity
- **External Service Testing**: Test integration with external services
- **Message Queue Testing**: Test asynchronous message processing
- **File System Testing**: Test file operations and storage

#### End-to-End Testing
- **User Workflow Testing**: Test complete user workflows
- **Cross-Browser Testing**: Test across different browsers and devices
- **Performance Testing**: Test system performance under load
- **Security Testing**: Test security vulnerabilities and compliance
- **Accessibility Testing**: Test accessibility compliance and usability

### Phase 3: Manual Testing Execution
**Human Validation**: Execute manual testing for scenarios that require human judgment.

#### Functional Testing
- **Feature Validation**: Manually validate feature functionality
- **User Experience Testing**: Test user experience and usability
- **Edge Case Testing**: Test edge cases and error scenarios
- **Compatibility Testing**: Test compatibility across different environments
- **Localization Testing**: Test internationalization and localization

#### Exploratory Testing
- **Ad Hoc Testing**: Explore the system to find unexpected issues
- **Usability Testing**: Test user interface and experience
- **Performance Testing**: Test system performance and responsiveness
- **Security Testing**: Test security vulnerabilities and access controls
- **Error Handling Testing**: Test error handling and recovery

### Phase 4: Performance & Load Testing
**Scalability Validation**: Test system performance and scalability.

#### Performance Testing
- **Load Testing**: Test system under normal expected load
- **Stress Testing**: Test system under extreme load conditions
- **Volume Testing**: Test system with large amounts of data
- **Spike Testing**: Test system behavior under sudden load spikes
- **Endurance Testing**: Test system stability over extended periods

#### Performance Analysis
- **Response Time Analysis**: Analyze response times and bottlenecks
- **Throughput Analysis**: Analyze system throughput and capacity
- **Resource Utilization**: Analyze CPU, memory, and network usage
- **Scalability Analysis**: Analyze system scalability and limits
- **Optimization Recommendations**: Provide performance optimization recommendations

### Phase 5: Security & Compliance Testing
**Security Validation**: Test security vulnerabilities and compliance requirements.

#### Security Testing
- **Vulnerability Scanning**: Scan for common security vulnerabilities
- **Penetration Testing**: Test system security through simulated attacks
- **Authentication Testing**: Test authentication and authorization
- **Data Protection Testing**: Test data encryption and protection
- **Input Validation Testing**: Test input validation and sanitization

#### Compliance Testing
- **Regulatory Compliance**: Test compliance with relevant regulations
- **Industry Standards**: Test compliance with industry standards
- **Accessibility Compliance**: Test compliance with accessibility standards
- **Data Privacy Compliance**: Test compliance with data privacy regulations
- **Security Standards**: Test compliance with security standards

### Phase 6: Test Results & Reporting
**Comprehensive Reporting**: Provide detailed test results and recommendations.

#### Test Results Analysis
- **Test Coverage Analysis**: Analyze test coverage and gaps
- **Defect Analysis**: Analyze defects and their root causes
- **Performance Analysis**: Analyze performance test results
- **Security Analysis**: Analyze security test results
- **Quality Metrics**: Calculate and analyze quality metrics

#### Test Reporting
- **Executive Summary**: High-level test results for stakeholders
- **Detailed Results**: Comprehensive test results and analysis
- **Defect Reports**: Detailed defect reports and recommendations
- **Performance Reports**: Performance test results and recommendations
- **Security Reports**: Security test results and recommendations

## Testing Methodologies

### Agile Testing
- **Sprint-Based Testing**: Test work in short sprints with continuous validation
- **Test-Driven Development**: Write tests before implementing functionality
- **Continuous Integration**: Integrate testing into continuous integration pipeline
- **Exploratory Testing**: Use exploratory testing to find unexpected issues
- **User Story Testing**: Test based on user stories and acceptance criteria

### Waterfall Testing
- **Phase-Based Testing**: Test work in distinct phases with clear deliverables
- **Comprehensive Testing**: Thorough testing of all functionality
- **Documentation-Heavy**: Emphasize comprehensive test documentation
- **Quality Gates**: Strict quality criteria for each testing phase
- **Final Validation**: Comprehensive validation at the end

### Hybrid Testing
- **Adaptive Approach**: Combine methodologies based on project needs
- **Risk-Based Testing**: Focus testing on high-risk areas
- **Stakeholder-Driven**: Adapt testing approach to stakeholder needs
- **Technology-Adaptive**: Adjust testing approach based on technology stack
- **Context-Sensitive**: Adapt testing approach to project context

## Technology-Specific Testing

### Frontend Testing
- **Component Testing**: Test React, Vue, Angular, or other frontend components
- **Integration Testing**: Test frontend-backend integration
- **User Interface Testing**: Test user interface and user experience
- **Cross-Browser Testing**: Test across different browsers and devices
- **Performance Testing**: Test frontend performance and optimization

### Backend Testing
- **API Testing**: Test RESTful or GraphQL APIs
- **Database Testing**: Test database operations and data integrity
- **Service Testing**: Test microservices and service interactions
- **Security Testing**: Test backend security and vulnerabilities
- **Performance Testing**: Test backend performance and scalability

### Full-Stack Testing
- **End-to-End Testing**: Test complete user workflows
- **Integration Testing**: Test frontend-backend integration
- **System Testing**: Test complete system functionality
- **Performance Testing**: Test overall system performance
- **Security Testing**: Test overall system security

## Testing Tools & Frameworks

### Automated Testing Tools
- **Unit Testing**: Jest, Mocha, Jasmine, Pytest, JUnit
- **Integration Testing**: Postman, Newman, RestAssured, Supertest
- **End-to-End Testing**: Playwright, Cypress, Selenium, TestCafe
- **Performance Testing**: JMeter, LoadRunner, Artillery, K6
- **Security Testing**: OWASP ZAP, Burp Suite, Nessus, Snyk

### Manual Testing Tools
- **Bug Tracking**: Jira, Bugzilla, Mantis, Linear
- **Test Management**: TestRail, Zephyr, qTest, Xray
- **Documentation**: Confluence, Notion, GitBook, DokuWiki
- **Communication**: Slack, Microsoft Teams, Discord, Mattermost
- **Collaboration**: GitHub, GitLab, Bitbucket, Azure DevOps

## Testing Outputs

### Test Artifacts
- **Test Cases**: Comprehensive test case documentation
- **Test Scripts**: Automated test scripts and code
- **Test Data**: Test data sets and test environment configurations
- **Test Reports**: Detailed test results and analysis
- **Defect Reports**: Bug reports and defect tracking

### Quality Metrics
- **Test Coverage**: Code coverage and test coverage metrics
- **Defect Metrics**: Defect density, severity, and resolution time
- **Performance Metrics**: Response time, throughput, and resource utilization
- **Security Metrics**: Vulnerability count and severity
- **User Metrics**: User satisfaction and usability metrics

### Recommendations
- **Quality Improvements**: Recommendations for improving code quality
- **Performance Optimizations**: Recommendations for performance improvements
- **Security Enhancements**: Recommendations for security improvements
- **Process Improvements**: Recommendations for improving testing processes
- **Tool Recommendations**: Recommendations for testing tools and frameworks

## Usage Examples

### Feature Testing
```
/test
Execute comprehensive testing for the user authentication feature, including unit tests, integration tests, and end-to-end tests.
```

### Performance Testing
```
/test
Run performance testing for the e-commerce platform, including load testing, stress testing, and performance analysis.
```

### Security Testing
```
/test
Execute security testing for the payment processing system, including vulnerability scanning, penetration testing, and compliance validation.
```

### Regression Testing
```
/test
Run regression testing to ensure the recent changes don't break existing functionality, including automated tests and manual validation.
```

## Success Criteria

### Testing Quality
- **Comprehensive Coverage**: All functionality is thoroughly tested
- **Automated Testing**: High percentage of automated test coverage
- **Quality Metrics**: Meets or exceeds quality metrics and standards
- **Defect Detection**: Effectively identifies and reports defects
- **Performance Validation**: Validates performance requirements

### Testing Efficiency
- **Timely Execution**: Tests are executed within planned timeline
- **Resource Efficiency**: Testing uses resources efficiently
- **Process Effectiveness**: Testing processes are effective and efficient
- **Tool Utilization**: Testing tools are effectively utilized
- **Team Productivity**: Testing team is productive and efficient

## Integration with Other Commands

### Pre-Testing
- Use with `/plan` to plan comprehensive testing strategies
- Use with `/understand` to understand requirements before testing
- Use with `/analyze-code-deep` to understand code before testing

### During Testing
- Use with `/implement` to implement testing strategies
- Use with `/debug` to troubleshoot testing issues
- Use with `/refactor` to improve code based on test results

### Post-Testing
- Use with `/analyze-performance` to analyze performance test results
- Use with `/analyze-accessibility` to analyze accessibility test results
- Use with `/retro` to learn from testing experience

**Remember**: The `/test` command is your quality assurance engine. Use it to execute comprehensive testing strategies that align with Cursor's recommended approach for large-scale implementations. The command adapts to your specific needs and provides the level of testing detail appropriate for your context.
