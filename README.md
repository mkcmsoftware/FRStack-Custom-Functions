# FRStack-Custom-Functions

FRStack version 3 Custom Functions allows you to extended the FRStack Menu, Hotkeys and Rest API with your own .NET DLL.<br />
<br />
You must have .Net programming skills and reviewed the Flex Radio System's FlexAPI SDK to utilized this feature. <br />
<br />
I recommend you download the Community version of Visual Studio 2019 to build your add-in.
https://visualstudio.microsoft.com/vs/community/

You will compile your project against the FRStack3 installation folder as the Sample demonstrates however you should download and reference the FlexAPI for a complete understanding. <br/> 
FlexAPI can be downloaded from FlexRadio's site 
https://www.flexradio.com/software/?_categories_dropdown=api
You must choose V3 of the API.


**Configuration**

The Custom Functions DLL is defined in the View, Options, Functions tab from within FRStack version 3
1) You will enter the DLL name without the DLL extension.
2) You will enter the fully qualified .NET Classname.

The DLL is initialized on FRStack startup and every time you connect to a radio.

At that time you will provide a list of function items to expose to the menu, hotkey and rest features.

Your DLL is copied into the FRStack installation folder which is typically "C:\Program Files (x86)\FRStack3"

<br/>
<br/>


**What is a Function item**
```
public class RadioFunctionItem
{
    public string menu { get; set; }
    public bool enabled { get; set; }
    public bool hidden { get; set; }
    public Func<object[], Task<object>> rfunct { get; set; }
}
```
***menu*** property is the menu item name where an underscore prefixes the keyboard shortcut. <br />
eg. "Hello _There" the T is the shortcut. <br />
<br />
***enabled*** property provides control of the menu item's state. False would grey out the menu item. <br />
<br />
***hidden*** property set true prevents the menu item creation however the function is still available via hotkey and Rest.<br />
<br />
***rfunc*** property sets the delegate method executed when menu item is clicked, hotkey is activated or REST API is called.
1) rfunc takes in an array of objects. 
The first element is a string "menu", "rest" or "hotkey". 
For Rest calls the second element is the optional Rest param.
2) rfunct is an asynchronous operation that returns an object. When invoked by a menu a string value is displayed to the FRStack user. 
When invoked by a Rest call the returned object is sent to the caller; so you can return a string, array or Json serializable object
<br />


<br />

**Menu**

Menu items are limited to what practically fits on the screen. Note if you set hidden to true then this item is only available from Rest or a Hotkey

<br />

**Hotkeys**

Hot keys are limited to the first 10 items you develop.

FUNC01,FUNC02,FUNC03,FUNC04,FUNC05,FUNC06,FUNC07,FUNC08,FUNC09,FUNC10,

<br />

**Rest**

There is no limit to the number of rest api items you develop.

FRStack Rest Command

http://localhost:13522/api/FRStack/CustomFunction/{cmd}?param={val}
{cmd} is the item's 1 based integer offset. So basically 1 to N. Or it is the menu name with any underscore removed.
 
 




<br/>
<br/>
