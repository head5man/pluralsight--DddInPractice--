### Aggregates

A root aggregate is a collection of entities which have a life-cycle dependency. If an entity cannot exist without another then it is a part of root aggregate. On the other hand if an entity can live on its own then it probably is another root or value object.

Entities inside comprise a cohesive group of classes.

As the domain evolves and new knowledge is gained don't hesitate to change the boundaries.

Beaware of creating too large aggregates this makes it harder to maintain consistency when dealing with multiple transactions.

Most aggregates consist of 1 or 2 entities. Usually 3 entities per aggregate is the max. Whereas the number of ValueObjects per aggregate has no such limit.

[cohesion and boundaries](http://bit.ly/1lisDBQ)

### Repositories

Is a pattern encapsulating all database communication.

For a domain model there should be a repository for each aggrate. And a repository should work with aggregate roots only.

### Bounded Contexts

* Separaration of the model into smaller ones.
* Boundary for the ubiquitous language.
* Span across all layers in the onion architecture.
* Explicit relationships between different bounded contexts.

```mermaid
C4Context
    title Context Diagram for DDD in Practice
    Component(shared_kernel, "Shared Kernel", "money")
    
    Boundary(context1, "Bounded Context")
    {
        Component(atm, "ATM")
    }

    Boundary(context2, "Bounded Context")
    {
        Component(sm, "SnackMachine")
    }
    Rel(atm, shared_kernel, "")
    Rel(sm, shared_kernel, "")
```

### Domain Events

Domain Event is an event that is significant to the domain model. Facilitate communication between bounded context and/or decouple entities within a single bound context. The main idea is to decouple bounded contexts so that bounded contexts do not become aware (dependent) of each other.

#### Domain Events Guidelines

Naming - past tense

Data - include as little data as possible

Event class - use primitive types instead of domain classes
```mermaid
C4Context
    title Introducing a New Bounded Context Management
    Component(shared_kernel, "Shared Kernel", "money")
    
    Boundary(context1, "Bounded Context")
    {
        Component(atm, "ATM")
    }

    Boundary(context0, "Bounded Context")
    {
        Component(management, "Management")
    }

    Boundary(context2, "Bounded Context")
    {
        Component(sm, "SnackMachine")
    }

    BiRel(atm, management, "")
    BiRel(sm, management, "")
    Rel(management, shared_kernel, "")
    Rel(atm, shared_kernel, "")
    Rel(sm, shared_kernel, "")
```