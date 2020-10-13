# Validation

Validation is done on different scope level

## Attribute Validation

For validating Behaviors and Condition Data we implemented the support of attribute validators, which can be used to validate specific fields.

The `StepAttributeValidator` checks for all Behaviors & Condition if there are any Attributes added which implement the interface `IAttributeValidator`. All found validators will be run and the reports given back to the ValidationHandler.
To implement your own AttributeValidation you have to implement `IAttributeValidator`.

### Existent `IAttributeValidator` implementations:
* OptionalValueAttribute: Flags the given field as optional and validation will not complain when the field is left empty.
* CheckForComponentAttribute: Can be attached to SceneReferences and checks if at least one Component listed in the attribute is added to the GameObject which is referenced.
* CheckForColliderAttribute: Checks if the SceneReference has at least one collider added.


