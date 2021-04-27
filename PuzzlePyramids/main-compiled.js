function _defineProperty(a,b,c){return b in a?Object.defineProperty(a,b,{value:c,enumerable:!0,configurable:!0,writable:!0}):a[b]=c,a}// Command to handle node babel build:
// npx babel main.js --presets @babel/preset-react --presets minify --plugins @babel/plugin-proposal-class-properties --watch --out-file main-compiled.js
// npx babel main.js --presets @babel/preset-react --presets minify --plugins @babel/plugin-proposal-class-properties --out-file main-compiled.js
class PuzzlePiece extends React.Component{constructor(a){super(a),this.state={value:""},this.handleChange=this.handleChange.bind(this),this.handleKeyDown=this.handleKeyDown.bind(this),this.handleFocus=this.handleFocus.bind(this),this.handleBlur=this.handleBlur.bind(this),this.getMyIndex=this.getMyIndex.bind(this),this.updateHookBuddy=this.updateHookBuddy.bind(this),this.myRef=React.createRef()}getMyIndex(){// Change focus here.
for(var a=document.querySelectorAll("div.puzzle.visible input.puzzle-piece.checkanswers"),b=-1,c=0;c<a.length;c++)if(a[c]==this.myRef.current){b=c;break}return[b,a]}updateHookBuddy(a){for(var b,c=document.querySelectorAll("div.puzzle.visible input.puzzle-piece.puzzle-piece-"+this.props.hook),d=0;d<c.length;d++)b=c[d],b!=this.myRef.current&&b.value!=a&&(b.value=a)}handleKeyDown(a){var b=a.target.value.slice(-1),c=event.keyCode||event.charCode,d=this.getMyIndex(),e=d[0],f=d[1];8==c&&(a.stopPropagation(),a.preventDefault(),null!=b&&""!=b&&!a.target.readOnly&&(this.setState({value:""}),this.myRef.current.value="",this.updateHookBuddy(""),a.target.parentElement.dataset.bakedin="false",a.target.parentElement.classList.remove("shaded")),e-=1,e+=f.length,e%=f.length,f[e].focus())}handleFocus(){for(var a,b=document.querySelectorAll("div.puzzle.visible input.puzzle-piece.puzzle-piece-"+this.props.hook),c=0;c<b.length;c++)a=b[c],a!=this.myRef.current&&a.classList.add("focused")}handleBlur(){for(var a,b=document.querySelectorAll("div.puzzle.visible input.puzzle-piece.puzzle-piece-"+this.props.hook),c=0;c<b.length;c++)a=b[c],a!=this.myRef.current&&a.classList.remove("focused")}handleChange(a){var b=a.target.value.slice(-1);if(null!=b&&""!=b){// Return here if the input is not actually a letter.
if(!/^[a-zA-Z]$/.test(b))return;b=b.toUpperCase()}this.setState({value:b}),this.myRef.current.value=b,this.updateHookBuddy(b);// Change focus here.
var c=this.getMyIndex(),d=c[0],e=c[1];null!=b&&""!=b&&(d+=1,d+=e.length,d%=e.length,e[d].focus())}render(){return/*#__PURE__*/React.createElement("input",{ref:this.myRef,className:"puzzle-piece puzzle-piece-"+this.props.hook+" "+("true"==this.props.checkAnswers?"checkanswers":""),type:"text",value:this.state.value,readOnly:"true"==this.props.readOnly,onKeyDown:this.handleKeyDown,onChange:this.handleChange,onFocus:this.handleFocus,onBlur:this.handleBlur,value:this.props.value})}}class Puzzle extends React.Component{constructor(a){super(a),this.state={number_of_letters:1,checking_answers:0,correct_answers:""},this.check_answers=this.check_answers.bind(this),this.reveal_letter=this.reveal_letter.bind(this),this.common_render=this.common_render.bind(this),this.toggleShading=this.toggleShading.bind(this)}toggleShading(a){var b=a.target;"TD"==b.tagName&&(b=b.parentNode),b.classList.contains("shaded")?b.classList.remove("shaded"):b.classList.add("shaded")}common_render(a=!1){return/*#__PURE__*/React.createElement("div",{className:"puzzle-header row d-flex justify-content-center py-3 border-bottom"},/*#__PURE__*/React.createElement("div",{className:"d-flex col-8 align-self-start"},/*#__PURE__*/React.createElement("h2",null,this.props.data.name)),/*#__PURE__*/React.createElement("div",{className:"col-4 align-items-end d-flex"},!a&&/*#__PURE__*/React.createElement("button",{className:"btn btn-primary",onClick:this.reveal_letter,type:"button"},"Reveal a Letter"),/*#__PURE__*/React.createElement("button",{className:"btn btn-primary",onClick:this.check_answers,type:"button"},"Check My Answers")))}reveal_letter(){for(var a=document.querySelectorAll("div.puzzle.visible input.puzzle-piece.checkanswers"),b=0;b<a.length;b++)if(a[b].value!=this.state.correct_answers[b]){var c=a[b],d=this.state.correct_answers[b];c.value=d,c.classList.add("shaded-orange");for(var e=void 0,f=0;f<c.classList.length;f++)if(c.classList[f].startsWith("puzzle-piece-")){e=c.classList[f];break}for(var g,h=document.querySelectorAll("div.puzzle.visible input.puzzle-piece."+e),b=0;b<h.length;b++)g=h[b],g!=c&&g.value!=d&&(g.value=d);return}}check_answers(){if(1!=this.state.checking_answers){for(var a=!0,b=document.querySelectorAll("div.puzzle.visible input.puzzle-piece.checkanswers"),c=[],d=[],e=0;e<b.length;e++)b[e].value==this.state.correct_answers[e]?d.push(b[e]):(a=!1,""!=b[e].value&&c.push(b[e]));if(a){for(var f,g=document.getElementsByTagName("td"),h=0;h<g.length;h++)if(g[h].textContent=="(Solve "+this.props.data.name+")"){f=g[h];break}null!=f&&(f.textContent=this.props.data.final_answer.toUpperCase())}for(var e=0;e<d.length;e++)d[e].classList.add("correct");for(var e=0;e<c.length;e++)c[e].classList.add("incorrect");this.state.checking_answers=1,setTimeout(function(){for(var a,b=document.querySelectorAll("div.puzzle input.puzzle-piece.checkanswers"),c=0;c<b.length;c++)a=b[c],(a.classList.contains("correct")||a.classList.contains("incorrect"))&&a.classList.add("checked-answer"),a.classList.remove("correct"),a.classList.remove("incorrect");setTimeout(function(){this.state.checking_answers=0;for(var a,b=document.querySelectorAll("div.puzzle input.puzzle-piece.checkanswers"),c=0;c<b.length;c++)a=b[c],a.classList.remove("checked-answer")}.bind(this),1e3)}.bind(this),2500)}}}class Puzzle_Box_Sum extends Puzzle{constructor(a){super(a);for(var b=0;b<this.props.data.answers.length;b++)this.state.correct_answers+=this.props.data.answers[b].toUpperCase();this.state.correct_answers+=this.props.data.final_answer.replace(/\s/g,"").toUpperCase()}render(){return/*#__PURE__*/React.createElement("div",{className:"container puzzle "+this.props.visible+" "+this.props.puzzleId},this.common_render(),/*#__PURE__*/React.createElement("div",{className:"row"},/*#__PURE__*/React.createElement("div",{className:"col"},/*#__PURE__*/React.createElement("h3",{className:"pb-4 border-bottom"},this.props.data.directions))),/*#__PURE__*/React.createElement("div",{className:"row pt-2"},/*#__PURE__*/React.createElement("div",{className:"col-12 col-sm-12 col-md-12 col-lg-6"},/*#__PURE__*/React.createElement("h2",null,"List of Clues"),/*#__PURE__*/React.createElement("table",{className:"box-traversal-table"},/*#__PURE__*/React.createElement("tbody",null,this.props.data.clues.map((a,b)=>/*#__PURE__*/React.createElement("tr",{key:this.props.data.name+"tr"+b,onClick:this.toggleShading},/*#__PURE__*/React.createElement("td",{className:"box-traversal-clue-td",key:this.props.data.name+"td"+b},a.value),/*#__PURE__*/React.createElement("td",{className:"box-traversal-clue-td",key:this.props.data.name+"td2"+b},a.clue)))))),/*#__PURE__*/React.createElement("div",{className:"col-12 col-sm-12 col-md-12 col-lg-6"},/*#__PURE__*/React.createElement("h2",null,"List of Words"),/*#__PURE__*/React.createElement("table",{className:"box-traversal-table"},/*#__PURE__*/React.createElement("tbody",null,this.props.data.solution_lengths.map((a,b)=>/*#__PURE__*/React.createElement("tr",{key:this.props.data.name+"tr"+b},/*#__PURE__*/React.createElement("td",{className:"box-traversal-clue-td",key:this.props.data.name+"td"+b},a),Array.from(this.props.data.answers[b]).map((a,c)=>/*#__PURE__*/React.createElement("td",{className:"box-fragment-td",key:this.props.data.name+"td"+b+c},/*#__PURE__*/React.createElement("div",{key:this.props.data.name+"div"+b+c},/*#__PURE__*/React.createElement(PuzzlePiece,{key:this.props.data.name+"p"+b+c,puzzleId:this.props.puzzleId,hook:this.props.puzzleId+String.fromCharCode(65+b)+this.state.number_of_letters++,readOnly:"false",checkAnswers:"true"})))))))),/*#__PURE__*/React.createElement("h3",{className:"pt-4"},"Solution"),Array.from(this.props.data.solution_boxes).map((a,b)=>/*#__PURE__*/React.createElement(PuzzlePiece,{key:this.props.data.name+"sol"+b,puzzleId:this.props.puzzleId,readOnly:"false",checkAnswers:"true",hook:this.props.puzzleId+a})))))}}class Puzzle_Box_Simple extends Puzzle{constructor(a){super(a);for(var b=0;b<this.props.data.clues.length;b++)this.state.correct_answers+=this.props.data.clues[b].answer.toUpperCase();this.state.correct_answers+=this.props.data.final_answer.replace(/\s/g,"").toUpperCase()}render(){var a=String.fromCharCode;return/*#__PURE__*/React.createElement("div",{className:"container puzzle "+this.props.visible+" "+this.props.puzzleId},this.common_render(),/*#__PURE__*/React.createElement("div",{className:"row"},/*#__PURE__*/React.createElement("div",{className:"py-4 col-12 col-sm-12 col-md-12 col-lg-12 border-bottom"},/*#__PURE__*/React.createElement("h3",{className:"pb-4 border-bottom"},this.props.data.directions),/*#__PURE__*/React.createElement("h2",{className:"py-2"},"Missing Words"),/*#__PURE__*/React.createElement("div",{className:"d-flex align-items-center justify-content-center drag_end"},/*#__PURE__*/React.createElement("table",{className:"box-traversal-table"},/*#__PURE__*/React.createElement("tbody",null,this.props.data.clues.map((b,c)=>/*#__PURE__*/React.createElement("tr",{key:this.props.data.name+"tr"+c},/*#__PURE__*/React.createElement("td",{className:"box-traversal-clue-td",key:this.props.data.name+"td"+c},b.clue),Array.from(b.answer).map((b,d)=>/*#__PURE__*/React.createElement("td",{className:"box-fragment-td "+(d+1==this.props.data.solution_column?"shaded-light":""),key:this.props.data.name+"td"+c+d},/*#__PURE__*/React.createElement("div",{key:this.props.data.name+"div"+c+d},/*#__PURE__*/React.createElement(PuzzlePiece,{key:this.props.data.name+"p"+c+d,puzzleId:this.props.puzzleId,hook:this.props.puzzleId+a(65+c)+this.state.number_of_letters++,readOnly:"false",checkAnswers:"true"})))))))))),/*#__PURE__*/React.createElement("div",{className:"col-12 col-sm-12 col-md-12 col-lg-12"},/*#__PURE__*/React.createElement("h3",{className:"py-4 border-bottom"},"Solution"),Array.from(this.props.data.final_answer).map((b,c)=>/*#__PURE__*/React.createElement(PuzzlePiece,{key:this.props.data.name+"sol"+c,puzzleId:this.props.puzzleId,readOnly:"true",hook:this.props.puzzleId+a(65+c)+(this.props.data.solution_column+this.props.data.clues[0].answer.length*c).toString()})))))}}class Puzzle_Fragment extends Puzzle{constructor(a){super(a),_defineProperty(this,"handleDragStart",a=>a.target.classList.contains("shaded")?void a.preventDefault():void(a.dataTransfer.setData(encodeUpperCase(JSON.stringify(a.target.innerText+"::"+a.target.id)),""),a.target.classList.add("shaded-light"))),_defineProperty(this,"handleDragEnd",a=>{a.target.classList.remove("shaded-light"),("move"==a.dataTransfer.dropEffect||"copy"==a.dataTransfer.dropEffect)&&a.target.classList.add("shaded")}),_defineProperty(this,"handleDragDrop",a=>{a.stopPropagation(),a.preventDefault();var b="INPUT"==a.target.nodeName?a.target.parentNode:a.target;// Drop Event
var c=!0,d=JSON.parse(decodeUpperCase(a.dataTransfer.types[0])),e=d.split("::"),f=e[0].split(""),g=e[1],h=document.getElementById(b.dataset.idNext),i=null;if(null==h?c=!1:(3==f.length&&(i=document.getElementById(h.dataset.idNext)),3==f.length&&null==i&&(c=!1)),c){if("true"==b.dataset.bakedin||"true"==h.dataset.bakedin||null!=i&&"true"==i.dataset.bakedin)return;b.dataset.bakedin="true",h.dataset.bakedin="true",null!=i&&(i.dataset.bakedin="true"),b.classList.remove("shaded"),h.classList.remove("shaded"),null!=i&&i.classList.remove("shaded"),b.dataset.sourceid=g}}),_defineProperty(this,"handleDragOver",a=>{a.stopPropagation(),a.preventDefault()}),_defineProperty(this,"handleDragEnter",a=>{if(a.stopPropagation(),a.preventDefault(),"DIV"==a.target.nodeName&&"INPUT"!=a.relatedTarget.tagName){// Drag Enter Event
var b=!0,c=JSON.parse(decodeUpperCase(a.dataTransfer.types[0])),d=c.split("::"),e=d[0].split(""),f=d[1],g=document.getElementById(a.target.dataset.idNext),h=null;if(null==g?b=!1:(3==e.length&&(h=document.getElementById(g.dataset.idNext)),3==e.length&&null==h&&(b=!1)),b){if("true"==a.target.dataset.bakedin||"true"==g.dataset.bakedin||null!=h&&"true"==h.dataset.bakedin)return;updateCharacter(a.target,e[0]),updateCharacter(g,e[1]),updateCharacter(h,e[2]),a.target.classList.add("shaded"),g.classList.add("shaded"),null!=h&&h.classList.add("shaded")}}}),_defineProperty(this,"handleDragExit",a=>{if(a.stopPropagation(),a.preventDefault(),"DIV"==a.target.nodeName&&"INPUT"!=a.relatedTarget.tagName){// Drag Exit Event
var b=!0,c=JSON.parse(decodeUpperCase(a.dataTransfer.types[0])),d=c.split("::"),e=d[0].split(""),f=d[1],g=document.getElementById(a.target.dataset.idNext),h=null;if(null==g?b=!1:(3==e.length&&(h=document.getElementById(g.dataset.idNext)),3==e.length&&null==h&&(b=!1)),b){if("true"==a.target.dataset.bakedin||"true"==g.dataset.bakedin||null!=h&&"true"==h.dataset.bakedin)return;updateCharacter(a.target,""),updateCharacter(g,""),updateCharacter(h,""),a.target.classList.remove("shaded"),g.classList.remove("shaded"),null!=h&&h.classList.remove("shaded")}}});for(var b=0;b<this.props.data.clues.length;b++)this.state.correct_answers+=this.props.data.clues[b].answer.toUpperCase();this.state.correct_answers+=this.props.data.final_answer.replace(/\s/g,"").toUpperCase(),this.handleDragStart=this.handleDragStart.bind(this),this.handleDragDrop=this.handleDragDrop.bind(this),this.handleDragOver=this.handleDragOver.bind(this),this.handleDragEnter=this.handleDragEnter.bind(this),this.handleDragExit=this.handleDragExit.bind(this),this.handleDragEnd=this.handleDragEnd.bind(this),this.fragmentToggleShading=this.fragmentToggleShading.bind(this)}fragmentToggleShading(a){console.log("fragmentToggleShading start"),console.log(a);var b=a.target;if("SPAN"==b.nodeName&&(b=b.parentNode),!b.classList.contains("shaded"))return void console.log("fragmentToggleShading not shaded, returning...");// Find the corresponding boxes to empty.
var c=document.querySelector("div.fragment-div[data-sourceid=\""+b.id+"\"]");if(console.log("looking for : div.fragment-div[data-sourceid=\""+b.id+"\"]"),null!=c){console.log("fragmentToggleShading found el"),updateCharacter(c,""),c.dataset.bakedin="false",c.classList.remove("shaded");var d=document.getElementById(c.dataset.idNext);if(updateCharacter(d,""),d.dataset.bakedin="false",d.classList.remove("shaded"),3==b.innerText.length){var e=document.getElementById(d.dataset.idNext);updateCharacter(e,""),e.dataset.bakedin="false",e.classList.remove("shaded")}c.dataset.sourceid=""}else console.log("fragmentToggleShading did not find el");b.classList.remove("shaded")}render(){var a=String.fromCharCode;return/*#__PURE__*/React.createElement("div",{className:"container puzzle "+this.props.visible+" "+this.props.puzzleId},this.common_render(!0),/*#__PURE__*/React.createElement("div",{className:"row py-4"},/*#__PURE__*/React.createElement("h3",{className:"pb-4 border-bottom"},this.props.data.directions),/*#__PURE__*/React.createElement("h2",{className:"pb-2"},"Missing Words"),/*#__PURE__*/React.createElement("div",{className:"pb-4 border-bottom d-flex align-items-center justify-content-center"},/*#__PURE__*/React.createElement("table",{className:"box-traversal-table"},/*#__PURE__*/React.createElement("tbody",null,this.props.data.clues.map((b,c)=>/*#__PURE__*/React.createElement("tr",{key:this.props.data.name+"tr"+c},/*#__PURE__*/React.createElement("td",{className:"box-traversal-clue-td",key:this.props.data.name+"td"+c},b.clue),Array.from(b.answer).map((b,d)=>/*#__PURE__*/React.createElement("td",{className:"box-fragment-td",key:this.props.data.name+"td"+c+d},/*#__PURE__*/React.createElement("div",{className:"fragment-div",onDrop:this.handleDragDrop,onDragEnter:this.handleDragEnter,onDragLeave:this.handleDragExit,onDragOver:this.handleDragOver,key:this.props.data.name+"div"+c+d,id:this.props.data.name+"div"+c+d,"data-id-next":this.props.data.name+"div"+c+(d+1),"data-bakedin":"false","data-hook":this.props.puzzleId+a(65+c)+this.state.number_of_letters},/*#__PURE__*/React.createElement(PuzzlePiece,{key:this.props.data.name+"p"+c+d,puzzleId:this.props.puzzleId,hook:this.props.puzzleId+a(65+c)+this.state.number_of_letters++,readOnly:"true",checkAnswers:"true"})))))))))),/*#__PURE__*/React.createElement("div",{className:"row pt-2 pb-4"},/*#__PURE__*/React.createElement("div",{className:"col-12 col-sm-12 col-md-12 col-lg-6"},/*#__PURE__*/React.createElement("h2",{className:"pb-4 border-bottom"},"Fragments"),/*#__PURE__*/React.createElement("h3",{className:"pt-2"},"Length 2:"),/*#__PURE__*/React.createElement("div",{className:"fragment-display d-flex align-items-center justify-content-center text-center"},this.props.data.fragments_2.map((a,b)=>/*#__PURE__*/React.createElement("div",{draggable:"true",onDragStart:this.handleDragStart,onDragEnd:this.handleDragEnd,className:"d-flex align-items-center justify-content-center text-center",onClick:this.fragmentToggleShading,key:this.props.data.name+"d2"+b,id:this.props.data.name+"d2"+b},/*#__PURE__*/React.createElement("span",{key:this.props.data.name+"p2"+b},a.toUpperCase())))),/*#__PURE__*/React.createElement("h3",{className:"pt-2"},"Length 3:"),/*#__PURE__*/React.createElement("div",{className:"fragment-display d-flex align-items-center justify-content-center text-center"},this.props.data.fragments_3.map((a,b)=>/*#__PURE__*/React.createElement("div",{draggable:"true",onDragStart:this.handleDragStart,onDragEnd:this.handleDragEnd,className:"d-flex align-items-center justify-content-center text-center",onClick:this.fragmentToggleShading,key:this.props.data.name+"d3"+b,id:this.props.data.name+"d3"+b},/*#__PURE__*/React.createElement("span",{key:this.props.data.name+"p3"+b},a.toUpperCase()))))),/*#__PURE__*/React.createElement("div",{className:"col-12 col-sm-12 col-md-12 col-lg-6"},/*#__PURE__*/React.createElement("h3",{className:"pb-4 border-bottom"},"Solution"),Array.from(this.props.data.solution_boxes).map((a,b)=>/*#__PURE__*/React.createElement(PuzzlePiece,{key:this.props.data.name+"sol"+b,puzzleId:this.props.puzzleId,readOnly:"true",hook:this.props.puzzleId+a})))))}}class Puzzle_WordSearch extends Puzzle{constructor(a){super(a);for(var b=0;b<this.props.data.clues.length;b++)this.state.correct_answers+=this.props.data.clues[b].answer.toUpperCase()}render(){var a=String.fromCharCode;return/*#__PURE__*/React.createElement("div",{className:"container puzzle "+this.props.visible+" "+this.props.puzzleId},this.common_render(),/*#__PURE__*/React.createElement("div",{className:"row"},/*#__PURE__*/React.createElement("div",{className:"py-4 col-12 col-sm-12 col-md-12 col-lg-6"},/*#__PURE__*/React.createElement("h3",{className:"pb-4 border-bottom"},this.props.data.directions),/*#__PURE__*/React.createElement("div",{className:"pt-2"},this.props.data.clues.map((b,c)=>/*#__PURE__*/React.createElement("div",{className:"wordsearch-div",key:this.props.data.name+"div"+c},/*#__PURE__*/React.createElement(PuzzlePiece,{key:this.props.data.name+"cl"+c,puzzleId:this.props.puzzleId,readOnly:"false",checkAnswers:"true",hook:this.props.puzzleId+a(65+c)+this.state.number_of_letters++}),/*#__PURE__*/React.createElement("span",{key:this.props.data.name+"p"+c},b.clue.toUpperCase()))))),/*#__PURE__*/React.createElement("div",{className:"py-4 col-12 col-sm-12 col-md-12 col-lg-6"},/*#__PURE__*/React.createElement("h3",{className:"pb-4 border-bottom"},"Grid of Letters"),/*#__PURE__*/React.createElement("div",{className:"py-2 border-bottom"},/*#__PURE__*/React.createElement("table",{className:"wordsearch-table"},/*#__PURE__*/React.createElement("tbody",null,this.props.data.grid.map((a,b)=>/*#__PURE__*/React.createElement("tr",{key:this.props.data.name+"tr"+b},Array.from(a).map((a,c)=>/*#__PURE__*/React.createElement("td",{key:this.props.data.name+"td"+b+c},a.toUpperCase()))))))),/*#__PURE__*/React.createElement("h3",{className:"py-4 border-bottom"},"Solution"),this.props.data.clues.map((b,c)=>/*#__PURE__*/React.createElement(PuzzlePiece,{key:this.props.data.name+"ans"+c,puzzleId:this.props.puzzleId,hook:this.props.puzzleId+a(65+c)+(c+1).toString(),readOnly:"true"})))))}}class Puzzle_Box extends Puzzle{constructor(a){super(a);for(var b=0;b<this.props.data.clues.length;b++)this.state.correct_answers+=this.props.data.clues[b].answer.toUpperCase()}render(){var a=String.fromCharCode;return/*#__PURE__*/React.createElement("div",{className:"container puzzle "+this.props.visible+" "+this.props.puzzleId},this.common_render(),/*#__PURE__*/React.createElement("div",{className:"row py-4"},/*#__PURE__*/React.createElement("h3",{className:"pb-4 border-bottom"},this.props.data.directions),/*#__PURE__*/React.createElement("div",{className:"pt-2"},/*#__PURE__*/React.createElement("table",{className:"box-table"},/*#__PURE__*/React.createElement("thead",null,/*#__PURE__*/React.createElement("tr",{className:"h-100"},/*#__PURE__*/React.createElement("th",{className:"h-100",scope:"col"}),Array.from(this.props.data.clues[0].answer).map((b,c)=>/*#__PURE__*/React.createElement("th",{key:this.props.data.name+"th"+c},/*#__PURE__*/React.createElement("div",{className:"h-100 d-flex align-items-center justify-content-center"},0==c&&/*#__PURE__*/React.createElement(PuzzlePiece,{key:this.props.data.name+"ans"+c,puzzleId:this.props.puzzleId,hook:this.props.puzzleId+a(60+c)+(1+c*this.props.data.clues[0].answer.length).toString(),readOnly:"true",value:this.props.data.first_letter.toUpperCase()}),0<c&&/*#__PURE__*/React.createElement(PuzzlePiece,{key:this.props.data.name+"ans"+c,puzzleId:this.props.puzzleId,hook:this.props.puzzleId+a(65+c-1)+(1+(c-1)*this.props.data.clues[0].answer.length).toString(),readOnly:"true"})))))),/*#__PURE__*/React.createElement("tbody",null,this.props.data.clues.map((b,c)=>/*#__PURE__*/React.createElement("tr",{className:"h-100",key:this.props.data.name+"tr"+c},/*#__PURE__*/React.createElement("td",{className:"h-100 box-traversal-clue-td",key:this.props.data.name+"td"+c},b.clue),Array.from(b.answer).map((b,d)=>/*#__PURE__*/React.createElement("td",{className:"h-100 box-td",key:this.props.data.name+"td"+c+d},/*#__PURE__*/React.createElement("div",{className:"h-100 d-flex align-items-center justify-content-center",key:this.props.data.name+"div"+c+d},/*#__PURE__*/React.createElement(PuzzlePiece,{key:this.props.data.name+"p"+c+d,puzzleId:this.props.puzzleId,hook:this.props.puzzleId+a(65+c)+this.state.number_of_letters++,readOnly:"false",checkAnswers:"true"})))))))))))}}class Puzzle_Box_Traversal extends Puzzle{constructor(a){super(a);for(var b=0;b<this.props.data.clues.length;b++)this.state.correct_answers+=this.props.data.clues[b].answer.toUpperCase();this.state.correct_answers+=this.props.data.final_answer.replace(/\s/g,"").toUpperCase()}render(){var a=String.fromCharCode;return/*#__PURE__*/React.createElement("div",{className:"container puzzle "+this.props.visible+" "+this.props.puzzleId},this.common_render(),/*#__PURE__*/React.createElement("div",{className:"row"},/*#__PURE__*/React.createElement("div",{className:"py-4 col-12 col-sm-12 col-md-12 col-lg-6"},/*#__PURE__*/React.createElement("h3",{className:"pb-2 border-bottom"},this.props.data.directions),/*#__PURE__*/React.createElement("table",{className:"table box-traversal-table"},/*#__PURE__*/React.createElement("tbody",null,this.props.data.clues.map((b,c)=>/*#__PURE__*/React.createElement("tr",{className:"h-100",key:this.props.data.name+"tr"+c},/*#__PURE__*/React.createElement("td",{className:"h-100 box-traversal-clue-td",key:this.props.data.name+"td"+c},b.clue),Array.from(b.answer).map((b,d)=>/*#__PURE__*/React.createElement("td",{className:"h-100 box-traversal-direction-td",key:this.props.data.name+"td"+c+d},/*#__PURE__*/React.createElement("div",{className:"row h-100 justify-content-center align-items-center",key:this.props.data.name+"div"+c+d},/*#__PURE__*/React.createElement("div",{className:"h-100 d-flex align-items-center col-6"},/*#__PURE__*/React.createElement(PuzzlePiece,{key:this.props.data.name+"p"+c+d,puzzleId:this.props.puzzleId,hook:this.props.puzzleId+a(65+c)+this.state.number_of_letters++,readOnly:"false",checkAnswers:"true"})),/*#__PURE__*/React.createElement("div",{className:"d-flex align-items-center justify-content-center col-6"},""!=this.props.data.box_structure[c][d]&&/*#__PURE__*/React.createElement("div",{className:"container"},/*#__PURE__*/React.createElement("div",{className:"row justify-content-center"},/*#__PURE__*/React.createElement("div",{className:"col-6 text-center"},/*#__PURE__*/React.createElement("span",{className:"box-traversal-direction"},this.props.data.box_structure[c][d][0])),/*#__PURE__*/React.createElement("div",{className:"col-6 text-center pr-2"},"u"==this.props.data.box_structure[c][d][1]&&/*#__PURE__*/React.createElement("i",{className:"bi bi-arrow-up"}),"d"==this.props.data.box_structure[c][d][1]&&/*#__PURE__*/React.createElement("i",{className:"bi bi-arrow-down"}),"l"==this.props.data.box_structure[c][d][1]&&/*#__PURE__*/React.createElement("i",{className:"bi bi-arrow-left"}),"r"==this.props.data.box_structure[c][d][1]&&/*#__PURE__*/React.createElement("i",{className:"bi bi-arrow-right"})))),""==this.props.data.box_structure[c][d]&&/*#__PURE__*/React.createElement("div",{className:"container"},/*#__PURE__*/React.createElement("div",{className:"row"},/*#__PURE__*/React.createElement("div",{className:"col-6 text-center"},/*#__PURE__*/React.createElement("span",{className:"box-traversal-direction"},this.props.data.box_structure[c][d][0])),/*#__PURE__*/React.createElement("div",{className:"col-6 text-center pr-2"},/*#__PURE__*/React.createElement("span",{className:"box-traversal-direction"}," "))))))))))))),/*#__PURE__*/React.createElement("div",{className:"py-4 col-12 col-sm-12 col-md-12 col-lg-1"}," "),/*#__PURE__*/React.createElement("div",{className:"py-4 col-12 col-sm-12 col-md-12 col-lg-5"},/*#__PURE__*/React.createElement("h3",{className:"pb-2 border-bottom"},"Solution"),/*#__PURE__*/React.createElement("div",{className:"pt-2"},Array.from(this.props.data.final_answer).map((b,c)=>" "==b?/*#__PURE__*/React.createElement("div",{key:this.props.data.name+"hd"+c,className:"horizontalgap"}," "):/*#__PURE__*/React.createElement(PuzzlePiece,{key:this.props.data.name+"ca"+c,puzzleId:this.props.puzzleId,readOnly:"false",checkAnswers:"true",hook:this.props.puzzleId+a(65+c)+this.state.number_of_letters++}))))))}}class Puzzle_Linked_FITB extends Puzzle{constructor(a){super(a);for(var b=0;b<this.props.data.section_1_clues.length;b++)this.state.correct_answers+=this.props.data.section_1_clues[b].answer.toUpperCase()}render(){return/*#__PURE__*/React.createElement("div",{className:"container puzzle "+this.props.visible+" "+this.props.puzzleId},this.common_render(),/*#__PURE__*/React.createElement("div",{className:"row"},/*#__PURE__*/React.createElement("div",{className:"py-4 col-12 col-sm-12 col-md-12 col-lg-6"},/*#__PURE__*/React.createElement("h3",{className:"pb-2 border-bottom"},this.props.data.section_1_directions),this.props.data.section_1_clues.map((a,b)=>/*#__PURE__*/React.createElement("div",{className:"border-bottom pb-2",key:this.props.data.name+"d"+b},/*#__PURE__*/React.createElement("h4",{key:this.props.data.name+"c"+b},a.clue),Array.from(a.answer).map((a,c)=>/*#__PURE__*/React.createElement(PuzzlePiece,{key:this.props.data.name+"c"+b+c,puzzleId:this.props.puzzleId,hook:this.props.puzzleId+String.fromCharCode(65+b)+this.state.number_of_letters++,readOnly:"false",checkAnswers:"true"}))))),/*#__PURE__*/React.createElement("div",{className:"py-4 col-12 col-sm-12 col-md-12 col-lg-6"},/*#__PURE__*/React.createElement("h3",{className:"pb-2 border-bottom"},this.props.data.section_2_directions),/*#__PURE__*/React.createElement("div",{className:"pt-2"},this.props.data.section_2_answers.map((a,b)=>" "==a?/*#__PURE__*/React.createElement("div",{key:this.props.data.name+"hd"+b,className:"horizontalgap"}," "):/*#__PURE__*/React.createElement(PuzzlePiece,{key:this.props.data.name+"ca"+b,puzzleId:this.props.puzzleId,hook:this.props.puzzleId+a.toUpperCase(),readOnly:"true"}))))))}}class App extends React.Component{constructor(a){super(a),this.state={currentPuzzle:0},this.switchPuzzle=this.switchPuzzle.bind(this)}switchPuzzle(a){for(var b,c=a.target.innerText.replace(/\s/g,"").toLowerCase(),d=document.querySelectorAll("div.puzzle"),e=0;e<d.length;e++)b=d[e],b.classList.contains(c)?(b.classList.remove("invisible"),b.classList.add("visible")):(b.classList.add("invisible"),b.classList.remove("visible"));d=document.querySelectorAll("a.nav-link");for(var b,e=0;e<d.length;e++)b=d[e],b.classList.remove("active");a.target.classList.add("active")}render(){return/*#__PURE__*/React.createElement("div",{className:"container"},/*#__PURE__*/React.createElement("header",{className:"d-flex flex-wrap justify-content-center mb-2 mt-2 pb-2 border-bottom"},/*#__PURE__*/React.createElement("ul",{className:"nav nav-pills"},puzzle_data.puzzles.map((a,b)=>/*#__PURE__*/React.createElement("li",{key:a.name+"_li",className:"nav-item"},/*#__PURE__*/React.createElement("a",{className:"nav-link "+(b==this.state.currentPuzzle?"active":""),key:a.name+"_button",onClick:this.switchPuzzle,type:"button"},a.name))))),puzzle_data.puzzles.map((a,b)=>{switch(a.type){case"linked_fitb":return/*#__PURE__*/React.createElement(Puzzle_Linked_FITB,{key:a.name,data:a,puzzleId:a.name.replace(/\s/g,"").toLowerCase(),visible:this.state.currentPuzzle==b?"visible":"invisible"});case"box_traversal":return/*#__PURE__*/React.createElement(Puzzle_Box_Traversal,{key:a.name,data:a,puzzleId:a.name.replace(/\s/g,"").toLowerCase(),visible:this.state.currentPuzzle==b?"visible":"invisible"});case"box":return/*#__PURE__*/React.createElement(Puzzle_Box,{key:a.name,data:a,puzzleId:a.name.replace(/\s/g,"").toLowerCase(),visible:this.state.currentPuzzle==b?"visible":"invisible"});case"wordsearch":return/*#__PURE__*/React.createElement(Puzzle_WordSearch,{key:a.name,data:a,puzzleId:a.name.replace(/\s/g,"").toLowerCase(),visible:this.state.currentPuzzle==b?"visible":"invisible"});case"fragment":return/*#__PURE__*/React.createElement(Puzzle_Fragment,{key:a.name,data:a,puzzleId:a.name.replace(/\s/g,"").toLowerCase(),visible:this.state.currentPuzzle==b?"visible":"invisible"});case"box_simple":return/*#__PURE__*/React.createElement(Puzzle_Box_Simple,{key:a.name,data:a,puzzleId:a.name.replace(/\s/g,"").toLowerCase(),visible:this.state.currentPuzzle==b?"visible":"invisible"});case"box_sum":return/*#__PURE__*/React.createElement(Puzzle_Box_Sum,{key:a.name,data:a,puzzleId:a.name.replace(/\s/g,"").toLowerCase(),visible:this.state.currentPuzzle==b?"visible":"invisible"});}}))}}class AppHeader extends React.Component{render(){return/*#__PURE__*/React.createElement("div",{className:"container"},/*#__PURE__*/React.createElement("header",{className:"d-flex flex-wrap justify-content-center py-3 border-bottom"},/*#__PURE__*/React.createElement("div",{className:"container"},/*#__PURE__*/React.createElement("div",{className:"row"},/*#__PURE__*/React.createElement("div",{className:"col-12 col-sm-12 col-md-12 col-lg-4"},/*#__PURE__*/React.createElement("a",{className:"d-flex align-items-center mb-3 mb-md-0 me-md-auto text-dark text-decoration-none",href:"/"},/*#__PURE__*/React.createElement("img",{className:"me-2",src:"./logo.png"}),/*#__PURE__*/React.createElement("span",{className:"fs-4"},puzzle_data.puzzle_metadata.header))),/*#__PURE__*/React.createElement("div",{className:"col-12 col-sm-12 col-md-12 col-lg-6 d-flex"},/*#__PURE__*/React.createElement("span",{className:"align-items-center fs-8 d-flex"},puzzle_data.puzzle_metadata.quote)),/*#__PURE__*/React.createElement("div",{className:"col-12 col-sm-12 col-md-12 col-lg-2 d-flex"},/*#__PURE__*/React.createElement("span",{className:"fs-8 d-flex justify-content-end align-items-center"},puzzle_data.puzzle_metadata.subheader))))))}}ReactDOM.render(React.createElement(AppHeader),document.querySelector("#AppHeader")),ReactDOM.render(React.createElement(App),document.querySelector("#App"));// Helper function for drag and drop.
function updateCharacter(a,b){if(null!=a){a.lastChild.value=b;for(var c,d=document.querySelectorAll("div.puzzle.visible input.puzzle-piece.puzzle-piece-"+a.dataset.hook),e=0;e<d.length;e++)c=d[e],c!=a.lastChild&&c.value!=b&&(c.value=b)}}const UPPERCASE_PREFIX="^{",UPPERCASE_SUFFIX="}^";function encodeUpperCase(a){return a.replace(/([A-Z]+)/g,`${UPPERCASE_PREFIX}$1${UPPERCASE_SUFFIX}`)}function decodeUpperCase(a){const b=a=>["",...a.split("")].join("\\");return a.replace(new RegExp(`${b(UPPERCASE_PREFIX)}(.*?)${b(UPPERCASE_SUFFIX)}`,"g"),(a,b)=>b.toUpperCase())}
