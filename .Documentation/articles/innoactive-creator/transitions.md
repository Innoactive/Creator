# Transitions

Transitions define when and how the training course transits from one step to the next. 
They are automatically triggered when all conditions are fulfilled. However this might not happen immediately! This is the case if behaviors or conditions of the previous steps need time to deactivate. Prominente examples of this are a `Delay Behavior` or an `Audio Behavior` that has the ‘Wait for completion’ attribute checked. Conditions might also create a delay, for example if they have a success animation.

To learn more about the life cycle of transitions, behaviors and conditions, check out section <a href="https://developers.innoactive.de/documentation/creator/latest/articles/developer/04-general-concepts.html#life-cycle" target="_blank">Life Cycle</a> in general concepts.

It is possible to have all conditions of several transitions be fulfilled at the same time which would cause several transitions to be triggered at the same time. In this case, the first of these transitions will be continued. This means that by ordering the transitions in the Step Inspector, you can define the priority of transitions for these cases.

Read more about our [default conditions](https://developers.innoactive.de/documentation/creator/latest/articles/innoactive-creator/default-conditions.html).
