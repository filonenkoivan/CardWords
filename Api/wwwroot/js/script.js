if(performance.navigation.type == 2){
  location.reload(true);
}

window.onload = async () => {
  const response = await fetch("collection/collections", {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  });
  const resp = await response.json();
  document.getElementById("userName").textContent = `Welcome, ${resp.name}`;

  resp.collections.forEach((element) => {
    addToCollection(element);
  });

  checkEmpty();

  //popup
  const openButtons = document.querySelectorAll("#openPopup");
  const closeButtons = document.querySelectorAll("#closePopup");
  const popup = document.getElementById("popup");
  const form = document.getElementById("cardForm");

  openButtons.forEach((button) => {
    button.addEventListener("click", () => {
      popup.style.display = "block";
    });
  });

  closeButtons.forEach((button) => {
    button.addEventListener("click", () => {
      popup.style.display = "none";
    });
  });

  form.addEventListener("submit", (event) => {
    event.preventDefault();
    const formData = new FormData(form);
    const card = {
      frontSideText: formData.get("frontSideText"),
      backSideText: formData.get("backSideText"),
      description: formData.get("description"),
    };
    popup.style.display = "none";
  });
  //
};

document
  .getElementById("collectionForm")
    .addEventListener("submit", async (event) => {
        event.preventDefault();
        const formData = {
            name: document.getElementById("collectionName").value,
        };

        const response = await fetch("collection/collections", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(formData),
        });
        const newCollection = await response.json();
        if (response.ok) {
            const cont = document.querySelector(".collection-content");

            const startHeight = cont.clientHeight;

            addToCollection(newCollection.item);

            const endHeight = cont.scrollHeight;

            cont.style.height = startHeight + "px";

            requestAnimationFrame(() => {
                cont.style.height = endHeight + "px";
            });

            document.querySelector("#collectionName").value = "";
            checkEmpty();

        } else {
            const script = document.createElement("script");
            script.src = "https://unpkg.com/sweetalert/dist/sweetalert.min.js";
            script.onload = () => {
                swal("Error", `${newCollection.message}`, "error");
            };
            document.head.appendChild(script);
        }
  });

// HTML
const html = `
  <div id="popup" style="display: none; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%); background: white; padding: 20px; box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);">
    <h2>Нова картка</h2>
    <form id="cardForm">
      <input name="frontSideText" placeholder="Текст передньої сторони" required /><br/>
      <input name="backSideText" placeholder="Текст задньої сторони" required /><br/>
      <textarea name="decsription" placeholder="Опис"></textarea><br/>
      <button type="submit" class = "submitWord">Зберегти</button>
      <button type="button" id="closePopup">Закрити</button>
    </form>
  </div>`;

document.body.insertAdjacentHTML("beforeend", html);

const addToCollection = (item) => {
  const collectionContainer = document.querySelector(".collection-content");
  const collectionElement = document.createElement("div");
  collectionElement.classList.add("collection");
  collectionElement.innerHTML = `
  <div class="collection-top">
    <a href="collection.html?id=${item.id}">${item.name}</a>
    <span>${item.createdTime}</span>
  </div>
  <div class="collection-top">
    <button class="addWord" id="openPopup">ADD WORD</button>
    <button class="deleteBtn">Delete collection</button>
  </div>
  <div class="first-words">
    ${
      item.cardList && item.cardList.length > 0
        ? item.cardList
            .map(card => card.frontSideText?.trim())
            .filter(text => text)
            .length > 1
          ? item.cardList
              .map(card => card.frontSideText?.trim())
              .filter(text => text)
              .join(', ')
          : item.cardList[0]?.frontSideText.trim()
        : ""
    }</div>`;
  collectionElement.setAttribute("data-id", `${item.id}`);
  collectionContainer.appendChild(collectionElement);

  collectionElement.querySelector(".addWord").addEventListener("click", function () {
    globalId = item.id;
    document.getElementById("popup").style.display = "block";
  });
};

document.addEventListener("click", async function (event) {
  if (event.target.classList.contains("deleteBtn")) {
    let parent = event.target.closest(".collection");
    if (parent) {
      let parentData = parent.getAttribute("data-id");

      const response = await fetch(`collection/collections/${parentData}`, {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
        },
      });

      if (response.ok) {
        // const cont = document.querySelector(".collection-content");
        // const contSecond = document.querySelector(".collections-container");
        // console.log(cont)
        // const startHeight = cont.clientHeight;
        // console.log(startHeight)

        parent.remove();

        // const endHeight = cont.scrollHeight;
        // console.log(endHeight)

        // cont.style.height = startHeight + "px";

        // requestAnimationFrame(() => {
        //     cont.style.height = endHeight + "px";
        //     // contSecond.style.height = endHeight + "px";
        // });


        
        checkEmpty();
      }
    }
  }
});
let globalId = 0;


document.addEventListener("click", async function (event) {
  if (event.target.classList.contains("addWord")) {
    let parent = event.target.closest(".collection");
    if (parent) {
      let form = document.querySelector("#cardForm");
      if (!form) return;

      let parentData = parent.getAttribute("data-id");
      globalId = parentData;

      if (!event.target.hasAttribute("data-listener")) {
        event.target.setAttribute("data-listener", "true");

        const submitButton = document.querySelector(".submitWord");
        
        if (!submitButton.hasAttribute("data-submitted")) {
          submitButton.setAttribute("data-submitted", "true");

          submitButton.addEventListener("click", async () => {
            const formData = new FormData(form);
            const frontSideText = formData.get("frontSideText");
            const backSideText = formData.get("backSideText");

            if (!frontSideText) {
              swal("Error","Please enter the front side text.", "error");
              return;
            }

            if (!backSideText) {
              swal("Error","Please enter the back side text.", "error");
              return;
            }

            const card = {
              frontSideText: frontSideText,
              backSideText: backSideText,
              decsription: formData.get("decsription"),
            };

            const response = await fetch(`collection/words/${globalId}`, {
              method: "POST",
              headers: {
                "Content-Type": "application/json",
              },
              body: JSON.stringify(card),
            });

            if (response.ok) {
              // Очищаємо форму після відправки
              document.querySelector("input[name='frontSideText']").value = "";
              document.querySelector("input[name='backSideText']").value = "";
              document.querySelector("textarea[name='decsription']").value = "";

              const firstWordsContainer = document.querySelector(`[data-id="${globalId}"]`).querySelector(".first-words");

              const existingWords = firstWordsContainer.innerHTML.trim();

              // Якщо є вже якісь слова, додаємо кому перед новим
              if (existingWords) {
                firstWordsContainer.innerHTML += `, ${card.frontSideText}`;
              } else {
                firstWordsContainer.innerHTML = `${card.frontSideText}`;
              }
            }
          });
        }
      }
    }
  }
});

function checkEmpty() {
  if(document.querySelector(".collection-content").querySelector(".collection") == null){
    const zeroItems = document.createElement("span")
    zeroItems.classList.add("empty-status")
    zeroItems.innerHTML = "No collections have been created yet, let's create them!";
    document.querySelector("#collectionsContainer").appendChild(zeroItems);
  }else{
    if(document.querySelector(".collections-container").querySelector(".empty-status") != null){
      document.querySelector(".collections-container").querySelector(".empty-status").remove();
    }
  }
}