# Copilot Instructions for RimWorld Modding Project

## Mod Overview and Purpose
The mod "FecalToCompost" for RimWorld introduces a sanitation and waste management system to enhance the realism and challenge of colony management. It involves structures and processes for managing sewage and converting it into compost, which can be used to fertilize crops, thus integrating an ecological cycle within the game.

## Key Features and Systems
- **Septic Tank System**: Introduces a `Building_SepticTank` class that manages sewage accumulation and processing.
- **Job System**: Includes `JobDriver_emptySewage` class to define behaviors related to emptying the septic tanks and `WorkGiver_emptySewage` class which dictates when and how these jobs are assigned to colonists.

## Coding Patterns and Conventions 
- **Core Language**: The project predominantly uses C# for mod development.
- **Namespace and Class Naming**: Classes are named with PascalCase, following C# conventions (e.g., `Building_SepticTank`, `JobDriver_emptySewage`).
- **XML Integration**: XML files should be used for defining in-game object properties, such as `ThingDef` and item behaviors.

## XML Integration
While XML summaries were not provided, typical XML files in RimWorld mods define in-game assets such as items, buildings, races, etc. For effective XML integration:
- Define object properties in XML files to avoid hardcoding values in C#.
- Ensure XML tags correspond accurately to the classes and fields they intend to modify in the game.

## Harmony Patching
- This mod would benefit from Harmony patching for modifying or extending base game functionalities without altering the core game files.
- Suggestions for Harmony Include:
  - Patch methods where the septic tank or compost actions need to interact with existing game logic.
  - Consider using `HarmonyPatch` attributes to target specific vanilla game methods you intend to tweak or overwrite.

## Suggestions for Copilot
1. **Generate Method Stubs**: Use Copilot to quickly scaffold additional methods for new building mechanics.
2. **XML Definitions**: Suggest XML templates for new in-game objects related to the mod.
3. **Refactoring and Documentation**: Encourage using Copilot for generating summaries and documentation for classes and methods, maintaining clarity.
4. **Harmony Patch Proposals**: Use Copilot to draft initial Harmony patches and explore possible points of intervention within the base game methods.
5. **Code Completion**: Utilize Copilot's auto-completion abilities to expedite coding while adhering to defined coding conventions.

By adhering to these guidelines and practices, Copilot can greatly assist in enhancing the mod's functionality, maintainability, and integration with the RimWorld ecosystem.
