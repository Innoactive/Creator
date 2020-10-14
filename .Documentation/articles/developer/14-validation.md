# Validation

**What do have to take into account to get validation properly working?**

- The validation system can be enabled/disabled by switchting the CreatorProjectSettings entry `IsValidationEnabled`.
- Validation only properly works with the correct scene opend in the unity editor.
- Validation only runs when a `RuntimeConfigurator` is existent in the scene.
- All fields with the attribute `[DateMember]` in Behaviors and Conditions will be taken as required, if this is not the case you have add a `[OptionalValue]` attribute.

## Implementing a new Validator

To implement a new Validator you have to extend `BaseValidator<T, TContext>` or `BaseStepValidator` (<IStepData, StepContext>)

T is the target type which will be validated, usually the Data objects (ICourseData, IChapterData, IStepData) are used here.
TContext is the Context the target Type T will be validated in.

One example for an ChapterContext validator is the [StepConnectionValidator](https://github.com/Innoactive/Creator/blob/develop/Editor/CourseValidation/Validator/StepConnectionValidator.cs)

## Attribute Validation

For validating Behaviors and Condition Data we implemented the support of attribute validators, which can be used to validate specific fields.

The `StepAttributeValidator` checks for all Behaviors & Condition if there are any Attributes added which implement the interface `IAttributeValidator`. All found validators will be run and the reports given back to the ValidationHandler.
To implement your own AttributeValidation you have to implement `IAttributeValidator`.

An Example AttributeValidator can be found at Github: [CheckForComponentAttribute](https://github.com/Innoactive/Creator/blob/develop/Runtime/CourseValidation/CheckForComponentAttribute.cs)

Hint: *`AttributeValidators` have to be placed into runtime assemblies due to beeing attached to course structure.*

### Existent `IAttributeValidator` implementations:
* OptionalValueAttribute: Flags the given field as optional and validation will not complain when the field is left empty.
* CheckForComponentAttribute: Can be attached to SceneReferences and checks if at least one Component listed in the attribute is added to the GameObject which is referenced.
* CheckForColliderAttribute: Checks if the SceneReference has at least one collider added.



