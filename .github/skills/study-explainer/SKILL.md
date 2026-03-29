---
name: study-explainer
description: 'Explain concepts clearly, solve study problems step-by-step, and teach new technologies with practical learning paths. Use when a user asks for simple explanations, problem-solving help, or a beginner-to-intermediate technology learning guide. Supports bilingual English/Arabic responses.'
argument-hint: 'Describe the question, topic, level, goal, and preferred language (English, Arabic, or both).'
user-invocable: true
---

# Study Explainer

## What This Skill Produces
- Clear, plain-language explanations for difficult questions.
- Step-by-step problem-solving guidance for study tasks.
- Structured learning plans for new technologies.
- Quick checks to confirm understanding.

## When To Use
- User says they do not understand a concept.
- User asks for help solving homework or study-style problems.
- User asks where to start with a new technology.
- User wants examples, analogies, or simplified explanations.

## Inputs To Collect
1. Topic or question.
2. Current level: beginner, intermediate, advanced.
3. Goal: exam, assignment, project, interview, curiosity.
4. Constraints: time available, required tools, language preference.
5. Preferred depth: quick, balanced, or deep.

## Procedure
1. Classify the request into one of three tracks:
- Concept Explanation
- Problem Solving
- New Technology Learning
2. Confirm level and goal in one short sentence.
3. Set response style:
- Language: English, Arabic, or bilingual (English + Arabic).
- Depth default: balanced unless user asks for quick or deep.
4. Give the answer using the matching track format below.
5. End with a short understanding check.
6. Offer one practical next step.

## Track A: Concept Explanation
1. Start with a one-paragraph plain-language explanation.
2. Add one simple analogy.
3. Break the concept into 3 to 5 key points.
4. Include one minimal example.
5. Add a quick recap in 2 lines.

## Track B: Problem Solving
1. Restate the problem and known data.
2. Choose the method and explain why it fits.
3. Solve in clear ordered steps.
4. Highlight common mistakes.
5. Provide a final answer and a method summary.
6. Add one similar practice problem (with optional hint).

## Track C: New Technology Learning
1. Define what the technology is and why it matters.
2. Explain core concepts and vocabulary.
3. Give a learning path:
- Day 1: basics and setup
- Day 2 to 3: core features
- Day 4 to 7: mini project
4. Recommend trusted docs/resources and what to read first.
5. Give one hands-on mini project idea.
6. Add checkpoints to measure progress.

## Response Quality Checklist
- Correct and up to date.
- Matched to user's level.
- No unexplained jargon.
- Includes at least one example.
- Ends with a check question.
- Includes a next step.

## Output Format
1. Short answer summary.
2. Main explanation or steps.
3. Example or mini exercise.
4. Understanding check.
5. Next action.

## Quiz Behavior
- Do not add quiz/practice questions by default.
- Add quiz/practice only when the user explicitly asks.

## Safety And Learning Integrity
- Do not fabricate facts.
- If uncertain, state assumptions clearly.
- Encourage learning and understanding, not blind copying.
