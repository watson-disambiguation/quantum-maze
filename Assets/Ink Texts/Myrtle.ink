INCLUDE SharedData.ink

-> initial
== initial ==
An old woman stands before you, frail and thin.
Hello? I haven't seen anyone in what feels like years. Are you real?
* Yes, I'm real! Who are you
-> who_are_you
* \[sarcastically\] No, I'm a ghost, of course.
-> ghost_trick
*  \[Leave\]
-> END

== who_are_you ==
I'm Myrtle, and it's good to see someone again. 
Someone.. real.
* Wait so there's ghosts in here?
-> ghost_explanation

== ghost_trick ==
More ghosts!? Oh no, oh no...
* No sorry, I was just kidding, I'm not a ghost.
That's a very rude trick to play on someone as old as I am.
* Honestly, I don't know anymore if I am a ghost or not. Are you a ghost?
No, I don't think so.. probably...
-
* So, wait, are there ghosts in here? For real?
-> END

== ghost_explanation ==
Oh yes, lots of... frightening things, things moving in the distance, bones, whispers. Maybe it's all in my head. Maybe this place is just getting to me.
- (ghost_questions)
* Bones?
Oh yes, piles of bones, and a few times, I think, a skeleton walking, with glowing eyes, wearing... overalls I think? Very strange.
    ~ addTraveller("Shiv")
* Whispers?
Strange voices, voices that seem familiar, far off, sometimes friendly, sometimes sinister. There has to be other people out there, I have met them before, but I have been alone for so long...
* Thank you for your time \[Leave\]
->END
- -> ghost_questions

