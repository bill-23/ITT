// Copyright 2016 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class DatabaseHandler : MonoBehaviour {

  public Text GameCode;
  public Text textToChange;

  void Start() {

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://ittseniordesign.firebaseio.com/");

        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        //reference.Child("test").Child("I think").SetValueAsync("Works");
    }


    public void writeNewUser(string email, string type)
    {
        //textToChange.text = "Sent Data";
        //DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        //reference.Child("test").Child("I think").SetValueAsync("Works");
        User user = new User(email, type);
        string json = JsonUtility.ToJson(user);
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("users").Child("user").SetRawJsonValueAsync(json);
    }

    // Exit if escape (or back, on mobile) is pressed.
    void Update() {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      Application.Quit();
    }

}
    
}
