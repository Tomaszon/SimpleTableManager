# SimpleTableManager

A console-based document editing software.

It is neet. It is fun.

<p align="center">
	<img alt="demo" src="https://github.com/Tomaszon/SimpleTableManager/blob/master/demo.png"/>
</p>

## What you can do with this fun little thing

### App
- Set rendering mode
- Undo/redo
- Auto save option

### Document
- Load
- Save
- Add/remove tables

### Table
- Resize
- Add/remove columns/rows
- Set column/row width/height
- Set view
- Change name
- Hide columns/rows

### Cell
- Set content
- Add comment
- Set format
	- Content color
	- Content alignment
	- Content padding
	- Border color
- Copy/paste content
- Copy/paste format

## Functions
In STM every cell that has value is calculated from a function. If its not given by default a string function will be created.

Some functions return a single, others multiple values, depending on the operator.

Some functions support additional 'named' arguments, that control the behavior of operation or formatting.

### Supported types and function operators
- Boolean
	- Constant
	- Not
	- And
	- Or

- Character
	- Constant
	- Concatenate
	- Join
	- Repeat

- Date/time/date and time
	- Constant
	- Summation
	- Now

- Integer/decimal (some operators only work with one or the other)
	- Constant
	- Negation
	- Absolute value
	- Summation
	- Subtraction
	- Average
	- Minimum/maximum
	- Floor
	- Ceiling
	- Rounding
	- Multiplication
	- Division
	- Remainder division
	- Bitwise and/or
	- Exponentiation
	- Square root
	- 2/10/E/N based logarithms

- String
	- Constant
	- Concatenate
	- Join
	- Length
	- Split
	- Trim
	- Blow

## Usage
STM has a basic CLI interface where commands can be typed in by hand or with the help of command autohelp by pressing the 'Tab' button.

The autohelp will iterate through the available command on the given command level. By pressing 'Space' a command level change can be applied.

By typing 'help' or by pressing the configured shortcut (F1 by default) help can be requested that describes the function and parameters with possible values.

<p align="center">
   <img alt="help" src="https://github.com/Tomaszon/SimpleTableManager/blob/master/help.png"/>
</p>

If the given command returns information, instead of modifying some data, that will be displayed under the table.

<p align="center">
   <img alt="show" src="https://github.com/Tomaszon/SimpleTableManager/blob/master/show.png"/>
</p>

## Basic settings
You can customize text and border foreground/background colors for display and selection.

Two border styles supported 'Modern' and 'Classic'.

Author and auto save options can be changed.

And it supports beep-boop on windows. Thats important too!

## Shortcuts
STM supports keyboard shortcuts for some actions, for example quick save, display refresh, undo, redo.

These shortcuts can be easily configured.

Note: some shortcuts may interfere with terminal shortcuts.

### More cool stuff to come!

Note: STM is a work in progress fun project. Unexpected errors and exceptions are practically bound to happen.