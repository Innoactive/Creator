# Validation System

The Validation System is part of our PRO offer. It allows you to easily find errors in your training courses. The system allows you to add new validations and also use the existing once for your own implemented Behaviors and Conditions. 

**What do have to take into account to get validation properly working?**

- The validation system can be enabled/disabled by switchting the CreatorProjectSettings entry `IsValidationEnabled`.
- Validation only runs when a `RuntimeConfigurator` is existent in the scene.
- All fields with the attribute `[DateMember]` in Behaviors and Conditions will be taken as required, if this is not the case you have add a `[OptionalValue]` attribute.

Overall validation is done by two different validator types, BaseValidation is scope specific for a `IData` objects, while the AttributeValidator is assigned to a field or property in any `BehaviorData` or `ConditionData`. Validation is run over the whole course data structure which means the BaseValidator can be run for `CourseData, ChapterData, StepData, BehaviorData, ConditionData`

## Validation via BaseValidator

To implement a new Validator you have to extend `BaseValidator<T, TContext>` or `BaseStepValidator`. Newly implemented Validator for Chapter- and StepContext will be automatically found and used while validating the specific Context.

* **T** is the target type which will be validated, usually the Data objects IChapterData or IStepData are used here.
* **TContext** is the Context the target Type T will be validated in, which has to be the fitting Context for the used type, for example StepContext for IStepData.

One example for an ChapterContext validator is the [StepConnectionValidator](https://github.com/Innoactive/Creator/blob/develop/Editor/CourseValidation/Validator/StepConnectionValidator.cs)

## Validation via Attribute Validation

For validating Behaviors and Condition Data we implemented the support of attribute validators, which can be used to validate specific fields.

The `StepAttributeValidator` checks for all Behaviors & Condition if there are any Attributes added which implement the interface `IAttributeValidator`. All found validators will be run and the reports given back to the ValidationHandler.
To implement your own AttributeValidation you have to implement `IAttributeValidator`.

An Example AttributeValidator can be found at Github: [CheckForComponentAttribute](https://github.com/Innoactive/Creator/blob/develop/Runtime/CourseValidation/CheckForComponentAttribute.cs)

Hint: *`AttributeValidators` have to be placed into runtime assemblies due to beeing attached to course structure.*

#### Existent `IAttributeValidator` implementations:
* OptionalValueAttribute: Flags the given field as optional and validation will not complain when the field is left empty.
* CheckForComponentAttribute: Can be attached to SceneReferences and checks if at least one Component listed in the attribute is added to the GameObject which is referenced.
* CheckForColliderAttribute: Checks if the SceneReference has at least one collider added.



