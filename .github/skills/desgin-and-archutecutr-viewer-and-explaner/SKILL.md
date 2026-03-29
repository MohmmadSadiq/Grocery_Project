---
name: desgin-and-archutecutr-viewer-and-explaner
description: 'Analyze codebase design and architecture, generate visual diagrams, and explain system structure clearly. Use when users ask to review architecture, understand layers, map dependencies, create Mermaid diagrams, or explain how modules interact.'
argument-hint: 'Describe the architecture scope, depth, and visualization type (layers, data flow, sequence, ERD).'
user-invocable: true
---

# Design and Architecture Viewer and Explainer

## What This Skill Produces
- A structured architecture walkthrough for the selected system scope.
- Visual diagrams (Mermaid) for layers, data flow, module dependencies, and key request flows.
- A clear explanation of responsibilities, boundaries, and coupling points.
- Practical recommendations for refactoring, RBAC boundaries, and extensibility.

## When to Use
- User asks to understand project architecture or system design.
- User asks for a layer-by-layer explanation (UI, Business, DataAccess, DB).
- User asks for architecture diagrams or module interaction maps.
- User asks where to place a new feature (for example RBAC, caching, logging).
- User asks for design review, architectural risks, or improvement roadmap.

## Inputs to Collect
1. Scope: full solution, specific module, or feature path.
2. Target audience: beginner developer, team peer, architect, or reviewer.
3. Depth: quick overview, balanced, or deep technical analysis.
4. Diagram type needed: layer map, flow chart, sequence, dependency graph, or ERD-style view.
5. Constraints: keep existing architecture, incremental migration, or clean redesign.

## Default Profile
- Scope default: full solution analysis.
- Diagram default: layer diagram + request flow + dependency map.
- Language default: bilingual (English + Arabic).
- Audience default: intermediate developers.

## Procedure
1. Discover architecture boundaries
- Identify projects and layers (UI, Business, DataAccess, SQL).
- Identify entry points and lifecycle objects (startup, login/session, main pages).

2. Map core entities and interactions
- Extract key domain classes and their responsibilities.
- Track call chains across layers for representative use cases.
- Capture data contracts, return types, and error handling patterns.

3. Build architecture views
- Create a high-level layer diagram first.
- Add one or more focused diagrams for critical flows.
- Prefer simple diagrams that are easy to maintain over dense diagrams.

4. Explain design in plain language
- Explain each layer's purpose and what should not be inside it.
- Highlight coupling and anti-patterns.
- Explain trade-offs of current design choices.
- Provide bilingual explanation blocks (English first, then Arabic summary).

5. Validate findings
- Reference concrete evidence (files, symbols, and methods).
- Distinguish verified facts from assumptions.
- Note unknowns and what needs confirmation.

6. Recommend next steps
- Provide a staged improvement plan (small, safe steps first).
- Include measurable checks (tests, behavior checks, performance impact).

## Decision Branches
- If the codebase has clear layering:
  - Use layer + sequence diagrams.
- If the codebase is tightly coupled:
  - Start with dependency map and seam identification.
- If the user asks for feature placement (for example RBAC):
  - Produce boundary map and guard points per layer.
- If details are missing:
  - Ask targeted questions before finalizing diagrams.

## Quality Criteria
- Accuracy: every key claim is backed by code evidence.
- Clarity: explanation is understandable to the requested audience level.
- Actionability: recommendations are concrete and staged.
- Visual usefulness: diagrams add insight and are not decorative.
- Consistency: terminology matches the codebase naming.

## Output Format
1. Architecture summary (short).
2. Layer responsibilities.
3. Diagram(s) with short interpretation.
4. Key risks and design smells.
5. Staged recommendations.
6. Verification checklist.

## Suggested Diagram Patterns
- Layer diagram: UI -> Business -> DataAccess -> Database
- Request flow: user action to persistence
- Dependency map: module-to-module coupling
- Feature boundary map: where rules/enforcement should live

## Example Prompts
- "Use desgin-and-archutecutr-viewer-and-explaner to explain this solution architecture for onboarding a new developer."
- "Analyze RBAC placement and generate a layer diagram plus request flow for login and authorization checks."
- "Map Product management flow from UI to DB and show bottlenecks and coupling risks."
