# Unity3D_LerpKit_Demo

<h2>Demo</h2>
<h5>This crudely constructed demo clearly illustrates the differences between using .Lerp(), .MoveTowards(), and .Lerp() + LerpKit</h5>
https://b-cancel.github.io/Unity3D_LerpKit_Demo/

<h2>Purpose</h2>
&nbsp;&nbsp;&nbsp;This tool was created so that it was possible to travel from point A to point B with a constant intuitive velocity without physics. 
<br>
&nbsp;&nbsp;&nbsp;Unity does have .Lerp() for Color, Mathf, Color32, Vector2, Vector3, Vector4, Material, and Quaternion... but to be able to do what is described above we would have to change the "t" value every frame. Unity also has .MoveTowards() for Mathf, Vector2, Vector3, and Vector4 but its hard to convert the "maxDistanceDelta" into some intuitive velocity. 
<br>
&nbsp;&nbsp;&nbsp;This is why the lerpKit was created.

<h2>How It's Done</h2>
&nbsp;&nbsp;&nbsp;As explained above to do this properly with .Lerp() we would have to update the "t" value every frame. If we want to travel said distance with a constant velocity then we simply need to calculate this "t" value every frame bassed on some velocity. A velocity required some guideDistane, and some guideTime. 

<h2>How To Use Lerp Kit</h2>
<h5>There are 2 ways to use Lerp Kit; each with their own pros and cons...</h5>
<ol>
  <li>
    using the "lerpKit" namespace
    <ul>
      <li>use the "using lerpKit;" directive → use "lerpHelper.function(...)" to call the function</li>
    </ul>
  </li>
  <li>
    using extension methods
    <ul>
      <li>create a "dummy instance" of (Mathf, Vector2, Vector3, Vector4, or Color) → use "dummyInstance.function(...)" to call the function</li>
      <li>create an "instance" of (Mathf, Vector2, Vector3, Vector4, or Color) → use "instance.function(..)" to call the function with "instance" replacing the first parameter [of the dummy instance method]</li> 
    </ul>
  </li>
</ol>

<h2>Enumerables</h2>
<ul>
  <li>
    updateLocation { fixedUpdate, Update };
    <ul>
      <li>it makes a difference where you place your "Type.lerp(...)" and you must indicate where you place it to properly calculate your lerp velocity</li>
      <li>NOTE: this can be read in by reading the call stack but its too expensive to be worth it</li>
    </ul>
  </li>
    <li>
    unitOfTime { frames, seconds };
    <ul>
      <li>this allows for easy conversion of your guide time into frames if you pass it seconds</li>
    </ul>
  </li>
    <li>
    guideDistance { distBetween_Other, distBetween_StartAndEnd, distBetween_CurrAndEnd, distBetween_StartAndCurr };
    <ul>
      <li>it makes sense to use one of these often. So you can simply chose an enumerable and the functions will calculate the distance you chose for you</li>
      <li>NOTE: distBetween_Other should only be used for using the Kit with Color in which case distBetween_Other will point to the largest possible distance between 2 points in the RGBA color space.</li>
      <li>To pass a custom distance you do not need the guideDistance enum</li>
    </ul>
  </li>
</ul>

<h2>Lerp Kit Documentation</h2>
The namespace that contains the implementation of every class used is called "lerpKit" <br>
The only class contained within that namespace is called "lerpHelper" <br>
The Functions within "lerpHelper" are… (all of these are public static float) <br>
<h4>For Guide Distance Calculation</h4>
<h6>Check the enumerables section to understand the guideDistance type</h6>
<ul>
  <li>calcGuideDistance(float startValue, float currValue, float endValue, guideDistance GD)</li>
  <li>calcGuideDistance(Vector2 startVect2, Vector2 currVector2, Vector2 endVector2, guideDistance GD)</li>
  <li>calcGuideDistance(Vector3 startVect3, Vector3 currVector3, Vector3 endVector3, guideDistance GD)</li>
  <li>calcGuideDistance(Vector4 startVect4, Vector4 currVector4, Vector4 endVector4, guideDistance GD)</li>
  <li>calcGuideDistance(float[] startValues, float[] currValues, float[] endValues, guideDistance GD)</li>
</ul>
<h4>For Lerp Value Calculation</h4>
<h6>LerpVelocity should be passed in distance per frame (either update or fixed update, depending on where your .Lerp() function is placed)</h6>
<ul>
  <li>calcLerpValue(float currValue, float endValue, float lerpVelocity_DperF)</li>
  <li>calcLerpValue(Vector2 currVector2, Vector2 endVector2, float lerpVelocity_DperF)</li>
  <li>calcLerpValue(Vector3 currVector3, Vector3 endVector3, float lerpVelocity_DperF)</li>
  <li>calcLerpValue(Vector4 currVector4, Vector4 endVector4, float lerpVelocity_DperF)</li>
  <li>calcLerpValue(float[] currValues, float[] endValues, float lerpVelocity_DperF)</li>
  <li>calcLerpValue(Color currColor, Color endColor, float lerpVelocity_DperF)</li>
</ul>
<h4>For Lerp Velocity Calculation</h4>
<h6>This will calculate Lerp Velocity in Distance per frame for you with more intuitive parameters</h6>
<ul>
  <li>calcLerpVelocity(float guideDistance, float timeToTravel_GD, unitOfTime UOT_GD, updateLocation UL)</li>
</ul>
<h4>Extra</h4>
<ul>
  <li>distance(Color color1, Color color2)
    <ul>
      <li>This was Created because Color has no definition of Distance. So I simply Imagined every possible color as a value in a 3D color cube and then I was able to use Vector3.Distance()</li>
    </ul>
  </li>
</ul>

<h2>Limitations</h2>
There are no lerpKit functions that will help you with Material.Lerp(), Quaternion.Lerp(), or Mathf.LerpAngle().
