    function CVVcechk() {
		//controlla se ci sono 3 valori nel campo CVV
        var cvv = /^[0-9]{3}$/
        if (!document.getElementById("CVV").value.match(cvv)) {
            return false
        }
        return true
    }
    function dataCechk() {
        var AAAA = new Date().getFullYear(); //anno corrente
        var MM = new Date().getMonth() + 1; //mese corrente
		
        if (document.getElementById("MM").value > 0 && document.getElementById("MM").value < 13) {					//il mese deve avere un valore compreso ra 1 e 12
            if (document.getElementById("AAAA").value == AAAA && document.getElementById("MM").value >= MM) {		//anno e mese uguali al corrente
                if (!CVVcechk()) {
                    return false
                }
                return true
            } else if (document.getElementById("AAAA").value > AAAA) { 												//anno superiore al corrente
                if (!CVVcechk()) {
                    return false
                }
                return true
            } else {																								//carta scaduta
                alert("Data di scadenza non valida");
				return false;
            }
        } else {
            alert( "Data di scadenza non valida");																	//mese inesistente
			return false
        }
        
    }
    function cechk() {
        if (document.getElementById("Accetta").checked) {
			
            if (document.getElementById("Visa").checked) {							//cechk visa card number
                
                var cardno = /^4[0-9]{12}(?:[0-9]{3})?$/;
                if (document.getElementById("CardNumb").value.match(cardno)) {
                    return dataCechk();
                }
                else {
                    alert("Not a valid Visa credit card number!\n");
					return false;
                }
            } else if (document.getElementById("MasterCard").checked) {				//cechk Master card card number
                var cardno = /^(?:5[1-5][0-9]{14})$/;
                if (document.getElementById("CardNumb").value.match(cardno)) {
                    return dataCechk();
                }
                else {
                    alert("Not a valid Mastercard number!");
					return false;
                }
            } else {																//check American Express card number
                var cardno = /^(?:3[47][0-9]{13})$/;
                if (document.getElementById("CardNumb").value.match(cardno)) {
                    return dataCechk();
                }
                else {
                    alert("Not a valid Amercican Express credit card number!");
					return false;
                }
            }
        } else {																	//per poter proseguire devi accettare i termini e condizioni del contratto
            alert("Per aquistare il corso devi accettare termini e condizioni del contratto");
			return false;
        }
    }