const apiBase = "http://localhost:5044/api";
function loadTickets(){
fetch(apiBase + "/tickets")
.then(res => res.json())
.then(data => {
let rows = "";
data.forEach(t => {
rows += `
<tr>
<td>${t.customerName}</td>
<td>${t.category}</td>
<td>${t.agent}</td>
<td>${t.issue}</td>
<td>
<select onchange="updateStatus(${t.ticketId}, this.value)">
<option ${t.status=="Open"?"selected":""}>Open</option>
<option ${t.status=="In Progress"?"selected":""}>In Progress</option>
<option ${t.status=="Closed"?"selected":""}>Closed</option>
</select>
</td>
</tr>
`;
});
let table = document.getElementById("ticketTable");
if(table){
table.innerHTML = rows;
}
});
}
function createTicket(){
let customer = document.getElementById("customer").value;
let issue = document.getElementById("issue").value;
let status = document.getElementById("status").value;
let agent = document.getElementById("agent").value;
let category = document.getElementById("category").value;
fetch(apiBase + "/tickets",{
method:"POST",
headers:{
"Content-Type":"application/json"
},
body:JSON.stringify({
customerName:customer,
issue:issue,
status:status,
agentId:parseInt(agent),
categoryId:parseInt(category)
})
})
.then(res=>res.text())
.then(msg=>{
alert(msg);
loadTickets();
});
}
function updateStatus(ticketId,newStatus){
fetch(apiBase + "/tickets/" + ticketId,{
method:"PUT",
headers:{
"Content-Type":"application/json"
},
body:JSON.stringify(newStatus)
})
.then(res=>res.text())
.then(msg=>{
alert(msg);
loadTickets();
});
}

/* CUSTOMER FUNCTIONS */
function addCustomer(){
let name=document.getElementById("customerName").value;
let email=document.getElementById("customerEmail").value;
fetch(apiBase+"/customers",{
method:"POST",
headers:{
"Content-Type":"application/json"
},
body:JSON.stringify({
name:name,
email:email
})
})
.then(res=>res.text())
.then(msg=>{
alert(msg);
loadCustomers();
});
}
function loadCustomers(){
fetch(apiBase+"/customers")
.then(res=>res.json())
.then(data=>{
let rows="";
data.forEach(c=>{
rows+=`
<tr>
<td>${c.name}</td>
<td>${c.email}</td>
</tr>
`;
});
let table=document.getElementById("customerTable");
if(table){
table.innerHTML=rows;
}
});
}
window.onload=function(){
loadTickets();
loadCustomers();
}