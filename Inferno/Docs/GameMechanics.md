Game Mechanics
==============

Mechanics are heavily inspired by D&D 5e, however they have been simplified to make them easier to work with in code,
and because we aren’t planning to make a 1 for 1 DnD clone.

# Combat

1) Turn order is determined (see initiative stat)
2) Player (who’s turn it is) get to do three things
    1) 1 Action
    2) Attack
    3) Defend
    4) Cast Spell
    5) Help Teammate
    6) Cunning Action
        1) Taunt - Try to draw agro
        2) Hide - Try to lose agro
    7) Run Away
3) 1 Class Action
4) Item Usage (optional)
5) Repeat until there is one team standing

# Modifiers (Concept)

A modifier is something that can be applied to a player by any means (ex. potion, event, spell, ect.). Basically, it
provides a +/- to a skill for a given duration.

Here’s a made up example:
Player casts shield of faith on themselves, it gives +1 to armor for n-turns, or until a condition occurs (ex. The
player leaves a certain area)
Or…
A rat bites the player and the player gets the plague, it now has a plagued modifier( -2 to all stats) until it gets
healed.

| Modifier | Description | Impacted Skills | Duration |
|----------|-------------|-----------------|----------|
| Plagued  | -2 to all   | All             | n-turns  |
|          |             |                 |          |

# Checks (Concept)

A check occurs when the player is going to interact with something that would require some level of skill.

For example, say the player finds a book on the ground and tries to read it, it happens to be a magic tomb that requires
them to have a lot of arcane knowledge, 15 skill points worth of it in fact. If the player’s arc. is >= then that
amount, they are able to do the thing. Modifiers from Items, modifiers and spells are also used when doing a check.

# Conditions (Concept)

Conditions are like modifiers, but they impact more than just the player’s skills.

For example, if a player is sitting in a chair you could grant them the condition “sitting”, when a player has the
sitting condition they are unable to walk

Another example, being knocked prone (aka getting knocked down) grants the “prone” condition to a player. When prone, a
player cannot take an action or class action on their turn.

# Player Stats

## Physical Armor Class (P. AC)

This represents the level of protection the character has against physical attacks. The attacker’s attack roll must beat
this number to hit the target.
Impacted By:

- Character Class (sometimes)
- Character Level
- Dex.
- Str.

## Magic Armor Class (M. AC)

This represents the level of protection the character has against magic attacks. The attacker’s attack roll must beat
this number to hit the target.

Impacted By:

- Character Class (sometimes)
- Character Level
- Dex.
- Arc.

## Hit Points (HP)

The player's health, dead at 0, “bloodied” at half health.

Impacted By:

- Character level
- Str.
- Buffs from items / other misc sources
- Mana Points (MP)
- Resource used to perform magical acts.

Impacted By:

- Character level
- Arc.
- Class (sometimes)

## Initiative (INI)

This is the metric that is used to determine attack order (turn based). Highest INI goes first, if there is a tie
between 2 players, highest PER goes first, if there is a tie again, highest DEX goes first

Impacted By:

- PER
- DEX

## Level (LVL)

The player’s level. Increased by gaining XP. When increased boosts MP & HP. At certain milestones, unlocks class based
“perks/skills”
Strength (STR)
Physical ability, and how tough you are.

Impacts:

- HP
- Melee Weapon Damage

## Dexterity (DEX)

- Measure of the players ability to perform feats that require finesses.

Impacts:

- Dodge Chance
- Chance to hit enemies with all weapons

## Arcane (ARC)

Measure of the players magical abilities

Impacts:

- MP
- Magic damage (from any source)
- Magic Resistance (from any source)

## Perception (PER)

How observant or detail oriented the player is

Impacts:

- Detect trap
- Detect deception
