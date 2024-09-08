document.addEventListener("DOMContentLoaded", function () {
    const seats = document.querySelectorAll('.row .seat:not(.sold)');
    const bookingDetails = document.getElementById('bookingDetails');
    const pricePerSeat = 70000; // Giá mỗi ghế
    const rows = ['H', 'G', 'F', 'E', 'D', 'C', 'B', 'A']; // Các hàng ghế
    let selectedSeats = [];

    var buttons = document.querySelectorAll('.btnchange');

    buttons.forEach(function (button) {
        button.addEventListener('click', function () {
            // Loại bỏ class 'selected' khỏi tất cả các button trước đó
            buttons.forEach(function (btn) {
                if (btn !== button) {
                    btn.classList.remove('selected');
                }
            });

            // Kiểm tra xem button đã được chọn trước đó hay chưa
            if (!button.classList.contains('selected')) {
                // Nếu chưa được chọn, thêm class 'selected' để đổi màu
                button.classList.add('selected');
            } else {
                // Nếu đã được chọn, loại bỏ class 'selected' để trở về trạng thái ban đầu
                button.classList.remove('selected');
            }
        });
    });

    seats.forEach(function (seat, index) {
        seat.addEventListener('click', function () {
            if (!seat.classList.contains('sold')) {
                if (!seat.classList.contains('selected')) {
                    if (canSelectSeat(index)) {
                        if (selectedSeats.length < 6) {
                            seat.classList.add('selected');
                            selectedSeats.push(index);
                        } else {
                            alert('Bạn chỉ có thể chọn tối đa 6 ghế!');
                        }
                    } else {
                        alert('KHÔNG ĐƯƠC ĐẶT GHẾ  XEN KẼ NHAU.');
                    }
                } else {
                    seat.classList.remove('selected');
                    const unselectedSeatIndex = selectedSeats.indexOf(index);
                    if (unselectedSeatIndex !== -1) {
                        selectedSeats.splice(unselectedSeatIndex, 1);
                    }
                }

                // Update booking details
                updateBookingDetails();
            }
        });
    });

    function canSelectSeat(index) {
        if (selectedSeats.length === 0) {
            return true;
        }

        const lastSelectedIndex = selectedSeats[selectedSeats.length - 1];
        const rowOfSelectedSeat = Math.floor(lastSelectedIndex / 8);
        const rowOfCurrentSeat = Math.floor(index / 8);

        // Check if the seat is consecutive with the last selected seat
        if (Math.abs(index - lastSelectedIndex) === 1) {
            if (rowOfSelectedSeat === rowOfCurrentSeat && !isSeatEmptyBetween(lastSelectedIndex, index)) {
                return true;
            }
        }

        // Check if there's no empty seat between the last selected seat and the current seat
        if (rowOfSelectedSeat !== rowOfCurrentSeat) {
            return true;
        }

        return false;
    }


    function isSeatEmptyBetween(startIndex, endIndex) {
        const step = Math.sign(endIndex - startIndex);
        let i = startIndex + step;
        while (i !== endIndex) {
            if (!seats[i].classList.contains('sold')) {
                return true; // There's an empty seat between selected seats
            }
            i += step;
        }
        return false;
    }

    function updateBookingDetails() {
        if (selectedSeats.length === 0) {
            bookingDetails.innerText = '';
        } else {
            const selectedSeatsInfo = selectedSeats.map(seatIndex => {
                const row = rows[Math.floor(seatIndex / 8)];
                const column = seatIndex % 8 + 1;
                return `${column}${row}`;
            });

            // Calculate total price
            const totalPrice = selectedSeats.length * pricePerSeat;

            // Format totalPrice to Vietnamese currency
            const formattedPrice = totalPrice.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });

            // Update booking details
            bookingDetails.innerHTML = `
      <div>Vị trí ghế: ${selectedSeatsInfo.join(', ')}</div>
      <div>Tổng tiền: ${formattedPrice}</div>
  `;

        }
    }
});
